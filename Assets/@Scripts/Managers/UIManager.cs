using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager
{
    private UI_Base _sceneUI;
    private Dictionary<string, UI_Base> _uiDic = new Dictionary<string, UI_Base>();

    public T GetSceneUI<T>() where T : UI_Base
    {
        return _sceneUI as T;
    }

    public T ShowSceneUI<T>() where T : UI_Base
    {
        if (_sceneUI != null)
            return GetSceneUI<T>();

        string key = typeof(T).Name + ".prefab";
        T ui = Managers.ResourceManager.Instantiate(key).GetOrAddComponent<T>();
        _sceneUI = ui;

        return ui;
    }

    public bool TryGetUIComponent<T>(out T uiComponent, Transform parent = null) where T : UI_Base
    {
        string key = typeof(T).Name;

        if (_uiDic.ContainsKey(key))
        {
            if (_uiDic[key] == null)
            {
                _uiDic.Remove(key);
            }
            else
            {
                uiComponent = _uiDic[key] as T;
                return true;
            }
        }

        GameObject prefab = Managers.ResourceManager.Load<GameObject>(key + ".prefab");
        if (!prefab)
        {
            Debug.LogError($"UI prefab 로드 실패 : {key}");
            uiComponent = null;
            return false;
        }

        GameObject obj = Managers.ResourceManager.Instantiate(prefab.name + ".prefab", parent);
        if (!obj.TryGetComponent<T>(out T component))
        {
            Debug.LogError($"Get UI Component 실패 : {key}");
            uiComponent = null;
            return false;
        }

        _uiDic.Add(key, component);
        uiComponent = component;
        return true;
    }

    public void Destroy()
    {
        if(_sceneUI != null)
        {
            GameObject.Destroy(_sceneUI.gameObject); 
            _sceneUI = null;
        }

        foreach(var ui in _uiDic.Values)
        {
            if(ui != null)
            {
                GameObject.Destroy(ui.gameObject);
            }
        }
        _uiDic.Clear(); 
    }
}
