using UnityEngine;
using UnityEngine.UI;

public class GachaPopupUIView : BaseUI
{
    [SerializeField] private Button ExitBackButton;
    [SerializeField] private Button ExitButton;


    private void OnEnable()
    {
        if (ExitBackButton != null)
        {
            ExitBackButton.onClick.AddListener(OnClickExitButton);
        }
        if (ExitButton != null)
        {
            ExitButton.onClick.AddListener(OnClickExitButton);
        }
    }

    private void OnDisable()
    {
        if (ExitBackButton != null)
        {
            ExitBackButton.onClick.RemoveListener(OnClickExitButton);
        }
        if (ExitButton != null)
        {
            ExitButton.onClick.RemoveListener(OnClickExitButton);
        }
    }

    private void OnClickExitButton()
    {
        GameManager.Instance.UIManager.CloseGacha();
    }

}
