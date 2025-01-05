using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

/// <summary>
/// Manages the character collider for consistent size across all characters.
/// Attached to PlayerPivot/CharacterSlot in the Main scene.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class CharacterCollider : MonoBehaviour
{
    static int s_HitHash = Animator.StringToHash("Hit"); // Animator hash for "Hit" animation
    static int s_BlinkingValueHash; // Shader property for blinking effect

    // Struct to store death event data (used for analytics or local stats)
    public struct DeathEvent
    {
        public string character; // Character name
        public string obstacleType; // Type of obstacle hit
        public string themeUsed; // Current theme
        public int coins; // Number of coins collected
        public int premium; // Premium currency collected
        public int score; // Final score
        public float worldDistance; // Distance traveled
    }

    public CharacterInputController controller; // Reference to character input controller
    public ParticleSystem koParticle; // Knockout effect

    [Header("Sound")]
    public AudioClip coinSound; // Sound for coin collection
    public AudioClip premiumSound; // Sound for premium currency collection

    public DeathEvent deathData { get { return m_DeathData; } } // Accessor for death data
    public new BoxCollider collider { get { return m_Collider; } } // Character collider
    public new AudioSource audio { get { return m_Audio; } } // Audio source

    [HideInInspector]
    public List<GameObject> magnetCoins = new List<GameObject>(); // Coins attracted by magnet

    public bool tutorialHitObstacle { get { return m_TutorialHitObstacle; } set { m_TutorialHitObstacle = value; } }

    protected bool m_TutorialHitObstacle; // Flag for tutorial collision
    protected bool m_Invincible; // Invincibility flag
    protected DeathEvent m_DeathData; // Stores death event data
    protected BoxCollider m_Collider; // Character collider component
    protected AudioSource m_Audio; // Audio source component
    protected float m_StartingColliderHeight; // Initial collider height

    protected readonly Vector3 k_SlidingColliderScale = new Vector3(1.0f, 0.5f, 1.0f); // Sliding collider size
    protected readonly Vector3 k_NotSlidingColliderScale = new Vector3(1.0f, 2.0f, 1.0f); // Default collider size

    protected const float k_MagnetSpeed = 10f; // Speed for magnet power-up
    protected const int k_CoinsLayerIndex = 8; // Layer index for coins
    protected const int k_ObstacleLayerIndex = 9; // Layer index for obstacles
    protected const int k_PowerupLayerIndex = 10; // Layer index for power-ups
    protected const float k_DefaultInvinsibleTime = 2f; // Default invincibility time

    protected void Start()
    {
        // Initialize components
        m_Collider = GetComponent<BoxCollider>();
        m_Audio = GetComponent<AudioSource>();
        m_StartingColliderHeight = m_Collider.bounds.size.y;
    }

    public void Init()
    {
        // Set up particle effect and initial states
        koParticle.gameObject.SetActive(false);
        s_BlinkingValueHash = Shader.PropertyToID("_BlinkingValue");
        m_Invincible = false;
    }

    public void Slide(bool sliding)
    {
        // Adjust collider size and position for sliding
        if (sliding)
        {
            m_Collider.size = Vector3.Scale(m_Collider.size, k_SlidingColliderScale);
            m_Collider.center -= new Vector3(0.0f, m_Collider.size.y * 0.5f, 0.0f);
        }
        else
        {
            m_Collider.center += new Vector3(0.0f, m_Collider.size.y * 0.5f, 0.0f);
            m_Collider.size = Vector3.Scale(m_Collider.size, k_NotSlidingColliderScale);
        }
    }

    protected void Update()
    {
        // Move magnet-attracted coins toward the player
        for (int i = 0; i < magnetCoins.Count; ++i)
        {
            magnetCoins[i].transform.position = Vector3.MoveTowards(magnetCoins[i].transform.position, transform.position, k_MagnetSpeed * Time.deltaTime);
        }
    }

    protected void OnTriggerEnter(Collider c)
    {
        // Handle different types of collisions (coins, obstacles, power-ups)
        if (c.gameObject.layer == k_CoinsLayerIndex)
        {
            HandleCoinCollision(c);
        }
        else if (c.gameObject.layer == k_ObstacleLayerIndex)
        {
            HandleObstacleCollision(c);
        }
        else if (c.gameObject.layer == k_PowerupLayerIndex)
        {
            HandlePowerupCollision(c);
        }
    }

    private void HandleCoinCollision(Collider c)
    {
        // Logic for collecting coins
        if (magnetCoins.Contains(c.gameObject))
            magnetCoins.Remove(c.gameObject);

        if (c.GetComponent<Coin>().isPremium)
        {
            Addressables.ReleaseInstance(c.gameObject);
            PlayerData.instance.premium += 1;
            controller.premium += 1;
            m_Audio.PlayOneShot(premiumSound);
        }
        else
        {
            Coin.coinPool.Free(c.gameObject);
            PlayerData.instance.coins += 1;
            controller.coins += 1;
            m_Audio.PlayOneShot(coinSound);
        }
    }

    private void HandleObstacleCollision(Collider c)
    {
        // Logic for handling obstacle collisions
        if (m_Invincible || controller.IsCheatInvincible())
            return;

        controller.StopMoving();
        c.enabled = false;

        Obstacle ob = c.GetComponent<Obstacle>();
        if (ob != null)
        {
            ob.Impacted();
        }
        else
        {
            Addressables.ReleaseInstance(c.gameObject);
        }

        if (TrackManager.instance.isTutorial)
        {
            m_TutorialHitObstacle = true;
        }
        else
        {
            controller.currentLife -= 1;
        }

        controller.character.animator.SetTrigger(s_HitHash);

        if (controller.currentLife > 0)
        {
            m_Audio.PlayOneShot(controller.character.hitSound);
            SetInvincible();
        }
        else
        {
            RecordDeathData(ob);
        }
    }

    private void HandlePowerupCollision(Collider c)
    {
        // Logic for collecting power-ups
        Consumable consumable = c.GetComponent<Consumable>();
        if (consumable != null)
        {
            controller.UseConsumable(consumable);
        }
    }

    private void RecordDeathData(Obstacle ob)
    {
        // Save death data for analytics or stats
        m_Audio.PlayOneShot(controller.character.deathSound);

        m_DeathData.character = controller.character.characterName;
        m_DeathData.themeUsed = controller.trackManager.currentTheme.themeName;
        m_DeathData.obstacleType = ob.GetType().ToString();
        m_DeathData.coins = controller.coins;
        m_DeathData.premium = controller.premium;
        m_DeathData.score = controller.trackManager.score;
        m_DeathData.worldDistance = controller.trackManager.worldDistance;
    }

    public void SetInvincibleExplicit(bool invincible)
    {
        m_Invincible = invincible;
    }

    public void SetInvincible(float timer = k_DefaultInvinsibleTime)
    {
        StartCoroutine(InvincibleTimer(timer));
    }

    protected IEnumerator InvincibleTimer(float timer)
    {
        // Temporarily make the character invincible with a blinking effect
        m_Invincible = true;

        float time = 0;
        float currentBlink = 1.0f;
        float lastBlink = 0.0f;
        const float blinkPeriod = 0.1f;

        while (time < timer && m_Invincible)
        {
            Shader.SetGlobalFloat(s_BlinkingValueHash, currentBlink);

            yield return null;
            time += Time.deltaTime;
            lastBlink += Time.deltaTime;

            if (blinkPeriod < lastBlink)
            {
                lastBlink = 0;
                currentBlink = 1.0f - currentBlink;
            }
        }

        Shader.SetGlobalFloat(s_BlinkingValueHash, 0.0f);
        m_Invincible = false;
    }
}
