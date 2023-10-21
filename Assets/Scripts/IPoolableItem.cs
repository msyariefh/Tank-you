using UnityEngine;
using UnityEngine.Pool;

public interface IPoolableItem 
{
    public IObjectPool<GameObject> Pool { get; set; }
}
