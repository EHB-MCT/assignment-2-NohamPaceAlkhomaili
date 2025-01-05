using UnityEngine;
using System.Collections;

/// <summary>
/// This class allows us to start Coroutines from non-MonoBehaviour scripts.
/// It creates a GameObject to run the coroutine on and ensures it persists across scenes.
/// </summary>
public class CoroutineHandler : MonoBehaviour
{
    static protected CoroutineHandler m_Instance; // Singleton instance of the CoroutineHandler.
    
    static public CoroutineHandler instance
    {
        get
        {
            // Create the instance if it doesn't already exist.
            if (m_Instance == null)
            {
                GameObject o = new GameObject("CoroutineHandler"); // Create a new GameObject named "CoroutineHandler".
                DontDestroyOnLoad(o); // Prevent the GameObject from being destroyed when loading new scenes.
                m_Instance = o.AddComponent<CoroutineHandler>(); // Attach the CoroutineHandler component to the GameObject.
            }

            return m_Instance;
        }
    }

    public void OnDisable()
    {
        // Destroy the GameObject if the instance is disabled.
        if (m_Instance)
            Destroy(m_Instance.gameObject);
    }

    static public Coroutine StartStaticCoroutine(IEnumerator coroutine)
    {
        // Starts a coroutine using the instance of CoroutineHandler.
        return instance.StartCoroutine(coroutine);
    }
}
