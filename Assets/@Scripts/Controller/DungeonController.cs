using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonController : BaseController
{
    private UI_DungeonPanel _dungeonPanel;
    private List<DungeonDataSO> _datas; 

    private ResourceManager _resourceManager;
    private CurrencyManager _currencyManager;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        _resourceManager = Managers.ResourceManager;
        _currencyManager = Managers.CurrencyManager;

        Managers.UIManager.TryGetUIComponent(out _dungeonPanel);

        _datas = _resourceManager.LoadAll<DungeonDataSO>();
        _datas = _datas.OrderBy(d => d.id).ToList();

        _dungeonPanel.CreateSlotInjection(CreateSlot); 
        return true; 
    }

    private void CreateSlot(Transform parent)
    {
        foreach(var data in _datas)
        {
            GameObject go = _resourceManager.Instantiate(Define.UIDUNGEONSLOT_PREFAB, parent);
            var slot = go.GetOrAddComponent<UI_DungeonSlot>();
            slot.transform.SetParent(parent);
            slot.SetSlot(data);
            slot.StartButtonInjection(StartDungeon);
            _dungeonPanel.onOpenUI -= slot.UpdateUI; 
            _dungeonPanel.onOpenUI += slot.UpdateUI; 
        }
    }

    private void StartDungeon(int id)
    {
        _currencyManager.SubtractCurrency(_datas[id].currencyType, 1); 
    
        var data = _datas[id];

        Managers.GameManager.onStartDungeon?.Invoke(data);
    }
}
