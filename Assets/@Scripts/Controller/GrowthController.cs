using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GrowthController : BaseController
{
    private UI_GrowthPanel _growthPanel;

    private Dictionary<Define.StatusType, GrowthData> _datas = new Dictionary<Define.StatusType, GrowthData>();
    private List<UI_GrowthSlot> _slots = new List<UI_GrowthSlot>();

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        List<GrowthData> datas = Managers.DataManager.growthCollection.growthDataList; 
        foreach (var data in datas)
        {
            _datas.Add(data.baseData.type,data);
        }

        Managers.UIManager.TryGetUIComponent(out _growthPanel);

        _growthPanel.CreateSlotsInjection(CreateSlots);
        _growthPanel.SetSlotsInjection(SetSlots);
        
        Managers.CurrencyManager.OnChangedCurrency -= SetSlots;
        Managers.CurrencyManager.OnChangedCurrency += SetSlots; 

        return true;
    }

    private void CreateSlots(Transform parent)
    {
        List<GrowthData> sortedDatas = _datas.OrderBy(data => data.Value.baseData.index).Select(data => data.Value).ToList();

        foreach (var data in sortedDatas)
        {
            GameObject go = Managers.ResourceManager.Instantiate(Define.UIGROWTHSLOT_PREFAB, parent);
            var slot = go.GetOrAddComponent<UI_GrowthSlot>();
            slot.transform.SetParent(parent, false);
            _slots.Add(slot);
            slot.SlotType = data.baseData.type;
            slot.OnClickUpgradeButtonInjection(UpgradeStatus);
            slot.UpdateUI(data);
        }
    }

    private void SetSlots()
    {
        foreach (var slot in _slots)
        {
            if (_datas.TryGetValue(slot.SlotType, out var data))
            {
                slot.UpdateUI(data);
            }
        }
    }

    public void UpgradeStatus(Define.StatusType type)
    {
        if (_datas.ContainsKey(type))
        {
            var data = _datas[type];

            if (!Managers.CurrencyManager.SubtractCurrency(Define.CurrencyType.Gold, data.price))
                return; 

            data.level++;
            data.totalIncrease += data.baseData.increase;
            data.totalPercentIncrease += data.baseData.percentIncrease;
            data.price += data.baseData.priceIncrease;

            Managers.GameManager.Player.PlayerData.UpgradeStatus(type, data.baseData.increase, data.baseData.percentIncrease);
            SetSlots();
        }
    }
}
