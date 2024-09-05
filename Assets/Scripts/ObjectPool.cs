using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : SingletonMonoBehaviour<ObjectPool>
{
    private Dictionary<string, PoolEntry> pool;

    private void OnEnable()
    {
        pool = new Dictionary<string, PoolEntry>();
        StartCoroutine(GarbageCheck());
    }

    private void OnDisable()
    {
        StopCoroutine(GarbageCheck());
    }

    // Handles checking if there are any game objects which can be removed from memory
    private IEnumerator GarbageCheck()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(60f);
            foreach (string key in new List<string>(pool.Keys))
            {
                if (pool[key].LastCall != DateTime.MinValue && DateTime.Now.Subtract(pool[key].LastCall).TotalSeconds >= 300f)
                {
                    while (pool[key].Available.Count > 0)
                    {
                        Destroy(pool[key].Available.Dequeue());
                    }
                    pool.Remove(key);
                }
            }
        }
    }

    // Instantiates a prefab to the scene
    private void InstantiateObject(GameObject objectToInstantiate)
    {
        GameObject newObject = Instantiate(objectToInstantiate);
        newObject.name = objectToInstantiate.name;
        ReturnToPool(newObject);
    }

    // "Spawns" the game object
    public GameObject Spawn(GameObject objectToSpawn, Vector3 position, Transform parent)
    {
        try
        {
            // If the queue is empty then create a new game object
            if (!pool.ContainsKey(objectToSpawn.name) || pool[objectToSpawn.name].Available.Count == 0)
            {
                InstantiateObject(objectToSpawn);
            }
            GameObject spawnedObject = pool[objectToSpawn.name].PopOut();
            spawnedObject.transform.SetParent(parent, false);
            spawnedObject.transform.localPosition = position;
            spawnedObject.SetActive(true);
            return spawnedObject;
        }
        catch (Exception e)
        {
            Debug.LogError($"Spawn failed due to {e}");
            return null;
        }
    }

    // "Spawns" the game object
    public GameObject Spawn(GameObject objectToSpawn, Transform parent)
    {
        try
        {
            // If the queue is empty then create a new game object
            if (!pool.ContainsKey(objectToSpawn.name) || pool[objectToSpawn.name].Available.Count == 0)
            {
                InstantiateObject(objectToSpawn);
            }
            GameObject spawnedObject = pool[objectToSpawn.name].PopOut();
            spawnedObject.transform.SetParent(parent, false);
            spawnedObject.SetActive(true);
            return spawnedObject;
        }
        catch (Exception e)
        {
            Debug.LogError($"Spawn failed due to {e}");
            return null;
        }
    }

    // Hides a game object from the scene and returns it to its pool
    public void Remove(GameObject objectToRemove)
    {
        ReturnToPool(objectToRemove);
    }

    // Performs an action before hiding the game object
    public void Remove(GameObject objectToRemove, Action action)
    {
        action.Invoke();
        ReturnToPool(objectToRemove);
    }

    // Places the game object back into its pool
    private void ReturnToPool(GameObject objectToReturn)
    {
        objectToReturn.transform.SetParent(transform, false);
        objectToReturn.SetActive(false);
        CheckPool(objectToReturn);
        pool[objectToReturn.name].PopIn(objectToReturn);
    }

    // Create a pool if one does not exist
    private void CheckPool(GameObject poolToCheck)
    {
        if (!pool.ContainsKey(poolToCheck.name))
        {
            pool.Add(poolToCheck.name, new PoolEntry());
        }
    }

}

public class PoolEntry
{
    private Queue<GameObject> available;
    private DateTime lastCall;

    public Queue<GameObject> Available { get => available; }
    public DateTime LastCall { get => lastCall; }

    public PoolEntry()
    {
        available = new Queue<GameObject>();
        lastCall = DateTime.MinValue;
    }

    public GameObject PopOut()
    {
        lastCall = DateTime.Now;
        return available.Dequeue();
    }

    public void PopIn(GameObject objectToEnqueue)
    {
        available.Enqueue(objectToEnqueue);
    }
}
