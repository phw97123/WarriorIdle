using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager 
{
    private UI_Base _sceneUI; 
    
    public T GetSceneUI<T>() where T : UI_Base
    {
        return _sceneUI as T; 
    }

    public T ShowSceneUI<T>() where T : UI_Base
    {
        if(_sceneUI != null) 
            return GetSceneUI<T>();

        string key = typeof(T).Name + ".prefab"; 
        T ui = Managers.ResourceManager.Instantiate(key).GetOrAddComponent<T>();
        _sceneUI = ui;

        return ui; 
    }    
}
