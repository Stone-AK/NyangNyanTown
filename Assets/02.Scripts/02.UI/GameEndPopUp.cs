using UnityEngine;
using UnityEngine.UI;

public class GameEndPopUp : BaseUI
{
    [SerializeField] Button GameEndYes;
    [SerializeField] Button GameEndNo;

    private void OnEnable()
    {
        if (GameEndYes != null)
            GameEndYes.onClick.AddListener(EndGame);

        if (GameEndNo != null)
            GameEndNo.onClick.AddListener(NoEndGame);
    }

    private void OnDisable()
    {
        if (GameEndYes != null)
            GameEndYes.onClick.RemoveListener(EndGame);

        if (GameEndNo != null)
            GameEndNo.onClick.RemoveListener(NoEndGame);
    }

    private void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    private void NoEndGame()
    {
        GameManager.Instance.UIManager.Close(UIType.GameEndPopUp);
    }
}
