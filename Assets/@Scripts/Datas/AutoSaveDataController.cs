using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AutoSaveDataController : BaseController
{
    private float saveInterval = 10f;
    private float saveTimer = 0f;

    private JsonManager _dataManager;

    private void Start()
    {
        _dataManager = Managers.JsonManager; 
    }

    void Update()
    {
        saveTimer += Time.deltaTime; 
        if(saveTimer >= saveInterval)
        {
            saveTimer = 0f;
            Managers.DataManager.SaveAllData(); 
        }
    }
}
