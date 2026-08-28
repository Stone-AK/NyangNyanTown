using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardSlotUIView : BaseUI
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text amountText;

    private string GoldKey = AddressableKey.GetPrefabKey(PrefabType.GoldSprite);
    public async Task Bind(RewardData reward)
    {
        switch (reward.Type)
        {
            case RewardType.Gold:
                amountText.text = reward.Amount.ToString();
                iconImage.sprite = await GameManager.Instance.ResourceManager.LoadAssetAsync<Sprite>(GoldKey, destroyCancellationToken);
                break;

            case RewardType.Cat:
                amountText.text = $"x{reward.Amount}";
                if (GameManager.Instance.DataManager.TryGetData(reward.Id, out CatInfoData catInfoData)) {
                    iconImage.sprite = await GameManager.Instance.ResourceManager.LoadAssetAsync<Sprite>(catInfoData.CatIconImgPath, destroyCancellationToken);
                }
                break;
        }
    }
}
