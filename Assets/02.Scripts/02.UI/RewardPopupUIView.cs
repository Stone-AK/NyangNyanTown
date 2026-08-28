using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class RewardPopupUIView : BaseUI
{
    [SerializeField] private Transform RewardSlotRoot;
    [SerializeField] private Button CloseButton;
    [SerializeField] private RewardSlotUIView rewardSlotPrefab;


    private List<RewardData> rewardDataList = new();
    private List<GameObject> _rewardSlotList = new();

    string slotPrfabKey = AddressableKey.GetUIKey(UIType.RewardSlotUI);

    private void OnEnable()
    {
        if (CloseButton != null)
        {
            CloseButton.onClick.AddListener(OnClickCloseButton);
        }
    }

    private void OnDisable()
    {
        CloseButton.onClick.RemoveListener(OnClickCloseButton);
    }

    private void OnClickCloseButton()
    {
        GameManager.Instance.UIManager.CloseRewardPopupUI();
    }

    public void Bind(List<RewardData> rewardDataList)
    {
        this.rewardDataList = rewardDataList;

        CreateRewardSlot().Forget();
    }


    private async UniTask CreateRewardSlot()
    {

        foreach (var slot in _rewardSlotList)
        {
            Destroy(slot.gameObject);
        }

        _rewardSlotList.Clear();

        foreach (var reward in rewardDataList)
        {


             var slot = await GameManager.Instance.ObjectManager.SpawnAsync(slotPrfabKey, RewardSlotRoot,Vector3.zero, Quaternion.identity);
            if (slot.gameObject.TryGetComponent<RewardSlotUIView>(out var slotUIView))
            {
                await slotUIView.Bind(reward);
                _rewardSlotList.Add(slot);

            }
        }
    }

}
