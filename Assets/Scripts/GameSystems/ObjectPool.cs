using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: reformat private and public fields
// TODO: make v3 and v2 coherent
// TODO: reformat order of functions
// TODO: make all changes to other classes via accessors
// TODO: hide all unnecessary public fields
// TODO: add comments
// TODO: make sure there are no "missle" words in this file
// TODO: update unity version
// TODO: clear obvious names such as pooled prefab

public class ObjectPool : Singleton<ObjectPool>
{
    public GameObject pooledPrefab;
    private Stack<Poolable> container = new Stack<Poolable>();
    public int poolSize;
    private void Start()
    {
        for(int i=0; i<poolSize; i++)
        {
            GameObject pooledObject = Instantiate(pooledPrefab, transform);
            pooledObject.SetActive(false);
            Poolable missleScript = pooledObject.GetComponent<Poolable>();
            container.Push(missleScript);
            missleScript.destroyed += Despawn;
        }
    }
    public virtual Poolable Spawn(Vector2 startPosition)
    {
        Poolable pooledObject = container.Pop();
        if (!pooledObject.isActiveAndEnabled)
        {
            pooledObject.transform.position = startPosition;
            pooledObject.gameObject.SetActive(true);
            return pooledObject;
        }
        return null;
    }
    public virtual void Despawn(Poolable missle)
    {
        missle.gameObject.SetActive(false);
        container.Push(missle);
    }
}
