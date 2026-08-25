using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIView : BaseUI
{
    [SerializeField] private Button Button_Start;
    [SerializeField] private Button Button_Exit;
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
            Button_Exit.onClick.RemoveListener(OnClickStartButtonAsync);
        }
    }


    private async void OnClickStartButtonAsync()
    {
        await GameManager.Instance.GameStartAsync();
        GameManager.Instance.UIManager.CloseMainMenuUI();
    }

    private void OnClickExitButton()
    {
        Debug.Log("게임 종료!");
    }
}
