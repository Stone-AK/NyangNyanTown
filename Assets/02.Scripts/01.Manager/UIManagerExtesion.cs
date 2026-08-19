using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public enum UIRootType
{
    None = 0,
    Main,
    Popup,
    VeryFront
}

public enum UIType
{
    //DNSimplePopup,
    MainUI,
    OverlayUI,
    BuildingPopUpUI,
    GachaPopupUI,
    CatEncyclopediaPopUp,
    LandUpGradeUI,
    MainMenuUI
}

public class BaseUI : MonoBehaviour
{
}

public static class UIManagerExtension
{
    public static async UniTask OpenMainUIAsync(this UIManager uiManager, CancellationToken cancellationToken = default)
    {
        await uiManager.OpenMainRootAsync(UIType.MainUI, cancellationToken);


        //await uiManager.OpenMainRootAsync(원하는 UI타입, cancellationToken);
        // Main 레이어에 UI를 소환하고 싶을 때

        // await uiManager.OpenPopupRootAsync(원하는 UI타입, cancellationToken);
        // Popup 레이어에 UI를 소환하고 싶을 때

        //await uiManager.OpenVeryFrontRootAsync(원하는 UI타입, cancellationToken);
        // VeryFront 레이어에 UI를 소환하고 싶을 때

    }
    public static async UniTask OpenLandUpGradeUIAsync(this UIManager uiManager, CancellationToken cancellationToken = default)
    {
        BaseUI baseUI = await uiManager.OpenPopupRootAsync(UIType.LandUpGradeUI, cancellationToken);
        if (baseUI is LandUpGradeUIView ui)
        {
            ui.Init(new LandUpGradeUIViewModel(GameManager.Instance.LandUpGradeService));
        }
    }
    public static void CloseMain(this UIManager uiManager)
    {
        uiManager.Close(UIType.MainUI);
    }
    //public static async UniTask OpenBuildingPopUpUIAsync(this UIManager uiManager, CancellationToken cancellationToken = default)
    //{
    //    await uiManager.OpenPopupRootAsync(UIType.BuildingPopUpUI, cancellationToken);
    //}
    
    public static async UniTask OpenOverlayAsync(this UIManager uiManager, CancellationToken cancellationToken = default)
    {
        await uiManager.OpenVeryFrontRootAsync(UIType.OverlayUI, cancellationToken);
    }

    public static void CloseOverlay(this UIManager uiManager)
    {
        uiManager.Close(UIType.OverlayUI);
    }

    public static async UniTask OpenGachaUIAsync(this UIManager uiManager, CancellationToken cancellationToken = default)
    {
        await uiManager.OpenPopupRootAsync(UIType.GachaPopupUI, cancellationToken);
    }

    public static void CloseGacha(this UIManager uiManager)
    {
        uiManager.Close(UIType.GachaPopupUI);
    }



    public static async UniTask OpenMainMenuUIAsync(this UIManager uiManager, CancellationToken cancellationToken = default)
    {
        await uiManager.OpenVeryFrontRootAsync(UIType.MainMenuUI, cancellationToken);
    }

    public static void CloseMainMenuUI(this UIManager uiManager)
    {
        uiManager.Close(UIType.MainMenuUI);
    }

    //UI를 생성하고 View를 가져오고 싶을 때 사용
    private static T GetView<T>(BaseUI baseUI, UIType uiType) where T : BaseUI
    {
        if (baseUI == null)
        {
            return null;
        }

        if (baseUI is not T view)
        {
            return null;
        }

        return view;
    }
}

