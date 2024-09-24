using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ResourceManager
{
    private Dictionary<string, UnityEngine.Object> _resources = new Dictionary<string, UnityEngine.Object>();

    public T Load<T>(string key) where T : UnityEngine.Object
    {
        if (_resources.TryGetValue(key, out UnityEngine.Object resource))
        {
            if (key.Contains(".sprite") && resource is Texture2D value)
            {
                Sprite sprite = Sprite.Create(value, new Rect(0, 0, value.width, value.height), new Vector2(0.5f, 0.5f));
                return sprite as T;
            }
            return resource as T;
        }
        else
            Debug.Log($"Failed to Load : {key}");

        return null;
    }

    public List<T> LoadAll<T>() where T : UnityEngine.Object
    {
        List<T> list = new List<T>();
        foreach (var resource in _resources.Values)
        {
            if (resource is T t)
            {
                list.Add(t);
            }
        }
        return list;
    }

    public GameObject Instantiate(string key, Transform parent = null, bool pooling = false)
    {
        GameObject prefab = Load<GameObject>($"{key}");
        if (prefab == null)
        {
            Debug.Log($"Failed to Load Prefab : {key}");
            return null;
        }

        if (pooling)
            return Managers.PoolManager.Pop(prefab);

        GameObject go = UnityEngine.Object.Instantiate(prefab, parent);
        go.name = prefab.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        if (Managers.PoolManager.Push(go))
            return;

        UnityEngine.Object.Destroy(go);
    }

    #region 어드레서블 

    // ResourceManager.cs
    public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
    {
        if (_resources.TryGetValue(key, out UnityEngine.Object resource))
        {
            callback?.Invoke(resource as T);
            return;
        }

        var asyncOperation = Addressables.LoadAssetAsync<T>(key);
        asyncOperation.Completed += (op) =>
        {
            _resources.Add(key, op.Result);
            callback?.Invoke(op.Result);
        };
    }

    public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : UnityEngine.Object
    {
        var opHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        opHandle.Completed += (op) =>
        {
            int loadCount = 0;
            int totalCount = op.Result.Count;

            foreach (var result in op.Result)
            {
                LoadAsync<T>(result.PrimaryKey, (obj) =>
                {
                    loadCount++;
                    callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                });
            }
        };
    }
    #endregion

    public void Destroy()
    {
        foreach (var resource in _resources.Values)
        {
            Addressables.Release(resource); 
        }
        _resources.Clear();
    }
}
