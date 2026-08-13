using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIView : BaseUI
{
    [SerializeField] private Button Button_Start;
    [SerializeField] private Button Button_Exit;


    private void OnEnable()
    {
        if (Button_Start != null) 
        {
            Button_Start.onClick.RemoveAllListeners();
            Button_Start.onClick.AddListener(OnClickStartButtonAsync);
        }

        if (Button_Exit != null)
        {
            Button_Exit.onClick.RemoveAllListeners();
            Button_Exit.onClick.AddListener(OnClickExitButton);
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
