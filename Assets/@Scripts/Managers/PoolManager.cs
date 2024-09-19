using System.Collections.Generic;
using UnityEngine;

class Pool
{
    private GameObject _prefab;
    private List<GameObject> _pools = new List<GameObject>();

    private Transform _root; 
    public Transform Root
    {
        get
        {
            if(_root == null)
            {
                GameObject go = new GameObject() { name = $"{_prefab.name}Root" };
                _root = go.transform; 
            }
            return _root;
        }
    }

    public Pool(GameObject prefab)
    {
        _prefab = prefab;
    }

    public void Push(GameObject go)
    {
        go.SetActive(false); 
        go.transform.SetParent(Root,false); 
        _pools.Add(go);
    }

    public GameObject Pop()
    {
        GameObject go; 
        if (_pools.Count > 0)
        {
            go = _pools[0]; 
            _pools.RemoveAt(0);
        }
        else
        {
            go = GameObject.Instantiate(_prefab);
            go.name = _prefab.name;
            go.transform.SetParent(Root,false);
        }

        go.SetActive(true); 
        return go;
    }
}

public class PoolManager 
{
    private Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();

    public GameObject Pop(GameObject prefab)
    {
        if (_pools.ContainsKey(prefab.name) == false)
            CreatePool(prefab);
        return _pools[prefab.name].Pop(); 
    }

    public bool Push(GameObject go)
    {
        if (!_pools.ContainsKey(go.name))
            return false;

        _pools[go.name].Push(go);
        return true; 
    }

    private void CreatePool(GameObject prefab)
    {
        Pool pool = new Pool(prefab);
        _pools.Add(prefab.name, pool); 
    }
}
