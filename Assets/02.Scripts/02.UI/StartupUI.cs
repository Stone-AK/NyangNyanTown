using Unity.VisualScripting;
using UnityEngine;

public class OverlayUI : BaseUI
{
    [SerializeField] private GameObject StartupCanvas;

    private async void Start()
    {
        await GameManager.Instance.StartGame();
        Destroy(StartupCanvas);
    }
}
