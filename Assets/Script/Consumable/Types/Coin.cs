using UnityEngine;
using System;

/// <summary>
/// CoinMagnet is a consumable item that attracts nearby coins to the player when active.
/// </summary>
public class CoinMagnet : Consumable
{
    // Defines the size of the box used for detecting nearby coins
    protected readonly Vector3 k_HalfExtentsBox = new Vector3(20.0f, 1.0f, 1.0f);
    
    // Layer mask for detecting only coins (Layer 8)
    protected const int k_LayerMask = 1 << 8;

    /// <summary>
    /// Returns the name of this consumable.
    /// </summary>
    public override string GetConsumableName()
    {
        return "Magnet";
    }

    /// <summary>
    /// Returns the type of this consumable.
    /// </summary>
    public override ConsumableType GetConsumableType()
    {
        return ConsumableType.COIN_MAG;
    }

    /// <summary>
    /// Returns the price of the consumable in standard currency.
    /// </summary>
    public override int GetPrice()
    {
        return 750;
    }

    /// <summary>
    /// Returns the premium cost of the consumable (0 in this case).
    /// </summary>
    public override int GetPremiumCost()
    {
        return 0;
    }

    // Stores detected colliders during the OverlapBoxNonAlloc call
    protected Collider[] returnColls = new Collider[20];

    /// <summary>
    /// Called every frame to check for nearby coins and attract them to the player.
    /// </summary>
    /// <param name="c">The character's input controller</param>
    public override void Tick(CharacterInputController c)
    {
        base.Tick(c);

        // Detects coins within the defined box around the player's character
        int nb = Physics.OverlapBoxNonAlloc(
            c.characterCollider.transform.position, 
            k_HalfExtentsBox, 
            returnColls, 
            c.characterCollider.transform.rotation, 
            k_LayerMask
        );

        // Loops through the detected colliders
        for (int i = 0; i < nb; ++i)
        {
            Coin returnCoin = returnColls[i].GetComponent<Coin>();

            // Adds non-premium coins to the magnet effect if they are not already magnetized
            if (returnCoin != null && !returnCoin.isPremium && !c.characterCollider.magnetCoins.Contains(returnCoin.gameObject))
            {
                returnColls[i].transform.SetParent(c.transform); // Attach the coin to the player
                c.characterCollider.magnetCoins.Add(returnColls[i].gameObject); // Add coin to the magnet list
            }
        }
    }
}
