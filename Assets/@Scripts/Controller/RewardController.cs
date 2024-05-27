using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class RewardController : BaseController
{
    private UI_RewardPopup _rewardPopup;
    private List<UI_RewardSlot> _rewardSlots;
    private List<RewardData> _datas;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _rewardSlots = new List<UI_RewardSlot>();
        _datas = new List<RewardData>();
        Managers.UIManager.TryGetUIComponent(out _rewardPopup);
        _rewardPopup.CreateSlotsInjection(CreateSlots);
        _rewardPopup.CloseUI(); 

        Managers.GameManager.OnRewardDataLoaded -= SetData; 
        Managers.GameManager.OnRewardDataLoaded += SetData; 

        return true;
    }

    private void CreateSlots(Transform parent)
    {
        for (int i = 0; i < _datas.Count; i++)
        {
            if (i < _rewardSlots.Count && _rewardSlots != null)
                _rewardSlots[i].SetData(_datas[i]);
            else
            {
                GameObject go = Managers.ResourceManager.Instantiate(Define.UIREWARDSLOT_PREFAB, parent, pooling: true);
                var slot = go.GetOrAddComponent<UI_RewardSlot>();
                slot.transform.SetParent(parent, false);
                slot.SetData(_datas[i]);
                _rewardSlots.Add(slot);
            }
        }

        for (int i = _datas.Count; i < _rewardSlots.Count; i++)
        {
            _rewardSlots[i].gameObject.SetActive(false);
        }
    }

    public void SetData(RewardData[] newDatas)
    {
        for(int i = 0; i<newDatas.Length; i++)
        {
            if(i<_datas.Count)
                _datas[i]  = newDatas[i];
            else
                _datas.Add(newDatas[i]);
        }

        if (newDatas.Length < _datas.Count)
            _datas.RemoveRange(newDatas.Length, _datas.Count - newDatas.Length); 
    }

    public UI_RewardPopup GetPopup()
    {
        return _rewardPopup; 
    }
}
