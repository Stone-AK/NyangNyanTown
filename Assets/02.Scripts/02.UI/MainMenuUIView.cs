using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIView : BaseUI
{
    [SerializeField] private Button Button_Start;
    [SerializeField] private Button Button_Exit;
    [SerializeField] private Button Button_Load;

    [SerializeField] private string BGMAudioId;


    private void OnEnable()
    {
        if (Button_Start != null) 
        {
            Button_Start.onClick.AddListener(OnClickStartButtonAsync);
        }

        if (Button_Exit != null)
        {
            Button_Exit.onClick.AddListener(OnClickExitButton);
        }
        if (Button_Load != null)
        {
            Button_Load.onClick.AddListener(OnClickLoadButton);
        }


        GameManager.Instance.AudioManager.PlayBGM(BGMAudioId);
    }

    private void OnDisable()
    {
        if (Button_Start != null)
        {
            Button_Start.onClick.RemoveListener(OnClickStartButtonAsync);
        }

        if (Button_Exit != null)
        {
            Button_Exit.onClick.RemoveListener(OnClickExitButton);
        }


        if (Button_Load != null)
        {
            Button_Load.onClick.RemoveListener(OnClickLoadButton);
        }
    }


    private async void OnClickStartButtonAsync()
    {
        await GameManager.Instance.GameStartAsync();
        GameManager.Instance.UIManager.CloseMainMenuUI();
    }

    private async void OnClickExitButton()
    {
        await GameManager.Instance.UIManager.OpenVeryFrontRootAsync(UIType.GameEndPopUp);
    }

    private async void OnClickLoadButton()
    {
        Debug.Log("게임 불러와잇!");

        if (!GameManager.Instance.SaveManager.TryReadGameData())
            return;

        await GameManager.Instance.GameStartAsync();
        await GameManager.Instance.SaveManager.LoadGameData();

        GameManager.Instance.UIManager.CloseMainMenuUI();
    }
}
