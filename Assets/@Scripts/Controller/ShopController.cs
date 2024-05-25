using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ShopController : BaseController
{
    private UI_ShopPanel _shopPanel; 
    private List<ShopDataSO> _datas;

    private CurrencyManager _currencyManager; 
    private ResourceManager _resourceManager;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _currencyManager = Managers.CurrencyManager;
        _resourceManager = Managers.ResourceManager;

        _datas = _resourceManager.LoadAll<ShopDataSO>();
        _datas = _datas.OrderBy(d => d.id).ToList();

        Managers.UIManager.TryGetUIComponent(out _shopPanel);
        _shopPanel.CreateSlotsInjection(CreateSlots); 

        return true; 
    }

    private void CreateSlots(Transform parent)
    {
        foreach(var data in _datas)
        {
            GameObject go = _resourceManager.Instantiate(Define.UISHOPSLOT_PREFAB, parent); 
            var slot = go.GetOrAddComponent<UI_ShopSlot>(); 
            slot.transform.SetParent(parent, false);
            slot.SetSlot(data);
            slot.BuyItemInjection(BuyItem);
            _shopPanel.onOpenUI -= slot.UpdateBuyButton; 
            _shopPanel.onOpenUI += slot.UpdateBuyButton; 
        }
    }

    private void BuyItem(int id)
    {
        var data = _datas[id];

        _currencyManager.SubtractCurrency(data.currencyType, data.price);
        _currencyManager.AddCurrency(data.getCurrencyType, data.count); 
    }
}
