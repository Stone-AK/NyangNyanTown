using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CatEncyclopediaSlotBtn : MonoBehaviour
{
    [SerializeField] private Image SlotImage;
    [SerializeField] private Button SlotButton;

    public async UniTask SetSlotImageAsync(string catIconAddress, bool isCollected)
    {
        if(isCollected)
        {
            Sprite loadedSprite 
                = await GameManager.Instance.ResourceManager.LoadAssetAsync<Sprite>(catIconAddress, destroyCancellationToken);

            if (loadedSprite != null)
                SlotImage.sprite = loadedSprite;
        }
        else
        {
            Sprite loadedSprite
                = await GameManager.Instance.ResourceManager.LoadAssetAsync<Sprite>("UI/CatIcon/UnknownCatIcon", destroyCancellationToken);

            if (loadedSprite != null)
                SlotImage.sprite = loadedSprite;
        }
    }

    public void BindOnClickSlotButton(Action onClickCallback)
    {
        if (SlotButton == null || onClickCallback == null)
            return;

        SlotButton.onClick.AddListener(() => onClickCallback.Invoke());
    }
}
