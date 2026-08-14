using Unity.VisualScripting;
using UnityEngine;

public class OverlayUI : BaseUI
{
    [SerializeField] private GameObject StartupCanvas;

    private async void Start()
    {
        gameObject.SetActive(true);
        await GameManager.Instance.FirstGameLoadingAsync();
        Destroy(StartupCanvas);
    }
}
