using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This class allows us to create multiple instances of a prefab and reuse them.
/// It helps optimize performance by avoiding the cost of destroying and re-creating objects.
/// </summary>
public class Pooler
{
    // Stack to store unused (free) instances of the prefab.
    protected Stack<GameObject> m_FreeInstances = new Stack<GameObject>();
    // The original prefab used to instantiate new objects if the pool is empty.
    protected GameObject m_Original;

    /// <summary>
    /// Initializes the pool with a specified number of instances of the original prefab.
    /// </summary>
    /// <param name="original">The prefab to pool.</param>
    /// <param name="initialSize">The number of instances to pre-create in the pool.</param>
    public Pooler(GameObject original, int initialSize)
    {
        m_Original = original;
        m_FreeInstances = new Stack<GameObject>(initialSize);

        // Pre-create the specified number of inactive instances and store them in the pool.
        for (int i = 0; i < initialSize; ++i)
        {
            GameObject obj = Object.Instantiate(original);
            obj.SetActive(false);
            m_FreeInstances.Push(obj);
        }
    }

    /// <summary>
    /// Retrieves an object from the pool, or instantiates a new one if the pool is empty.
    /// </summary>
    /// <returns>The retrieved object.</returns>
    public GameObject Get()
    {
        return Get(Vector3.zero, Quaternion.identity); // Default position and rotation.
    }

    /// <summary>
    /// Retrieves an object from the pool and sets its position and rotation.
    /// </summary>
    /// <param name="pos">The position to set on the object.</param>
    /// <param name="quat">The rotation to set on the object.</param>
    /// <returns>The retrieved object.</returns>
    public GameObject Get(Vector3 pos, Quaternion quat)
    {
        // Get an object from the pool or instantiate a new one if the pool is empty.
        GameObject ret = m_FreeInstances.Count > 0 ? m_FreeInstances.Pop() : Object.Instantiate(m_Original);

        ret.SetActive(true); // Activate the object.
        ret.transform.position = pos; // Set its position.
        ret.transform.rotation = quat; // Set its rotation.

        return ret;
    }

    /// <summary>
    /// Returns an object to the pool, making it inactive and available for reuse.
    /// </summary>
    /// <param name="obj">The object to return to the pool.</param>
    public void Free(GameObject obj)
    {
        obj.transform.SetParent(null); // Detach from any parent.
        obj.SetActive(false); // Deactivate the object.
        m_FreeInstances.Push(obj); // Add the object back to the pool.
    }
}
