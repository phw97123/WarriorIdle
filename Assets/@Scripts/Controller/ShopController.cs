using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopController : BaseController
{
    private UI_ShopPanel _shopPanel; 
    private List<UI_ShopSlot> _slots = new List<UI_ShopSlot>();
    private List<ShopDataSO> datas;

    private CurrencyManager _currencyManager; 
    private ResourceManager _resourceManager;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _currencyManager = Managers.CurrencyManager;
        _resourceManager = Managers.ResourceManager;

       Managers.UIManager.TryGetUIComponent(out _shopPanel);
        _shopPanel.CreateSlotsInjection(CreateSlots); 

        datas = _resourceManager.LoadAll<ShopDataSO>();
        return true; 
    }

    private void CreateSlots(Transform parent)
    {
        foreach(var data in datas)
        {
            GameObject go = _resourceManager.Instantiate(Define.UISHOPSLOT_PREFAB, parent); 
            var slot = go.GetOrAddComponent<UI_ShopSlot>(); 
            slot.transform.SetParent(parent, false);
            slot.SetSlot(data);
            slot.BuyItemInjection(BuyItem); 
            _slots.Add(slot); 
        }
    }

    private void BuyItem(int id)
    {
        var data = datas[id];

        _currencyManager.SubtractCurrency(data.currencyType, data.price);
        _currencyManager.AddCurrency(data.getCurrencyType, data.count); 
    }
}
