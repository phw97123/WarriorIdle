using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AutoSaveDataController : BaseController
{
    private float saveInterval = 10f;
    private float saveTimer = 0f;

    private DataManager _dataManager;

    private void Start()
    {
        _dataManager = Managers.DataManager; 
    }

    void Update()
    {
        saveTimer += Time.deltaTime; 
        if(saveTimer >= saveInterval)
        {
            saveTimer = 0f;
            SaveAllData(); 
        }
    }

    private void SaveAllData()
    {
        _dataManager.SaveData(Managers.GameManager.Player.PlayerData, "PlayerData");
        _dataManager.SaveData(Managers.GameManager.stageData, "StageData");
        _dataManager.SaveData(Managers.GameManager.skillDataCollection, "SkillDataCollection");
        _dataManager.SaveData(Managers.GameManager.equipmentCollection, "EquipmentCollection");
        _dataManager.SaveData(Managers.GameManager.growthCollection, "GrowthDataCollection");
        _dataManager.SaveData(Managers.CurrencyManager.currencyDataCollection, "CurrencyDataCollection");

        Debug.Log("데이터 저장 완료"); 
    }
}
