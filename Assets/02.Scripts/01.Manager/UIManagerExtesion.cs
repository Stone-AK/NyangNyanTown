using Cysharp.Threading.Tasks;
using System;
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
    OverlayUI
}

public class BaseUI : MonoBehaviour
{
}

public static class UIManagerExtension
{
    public static async UniTask OpenMainUIAsync(this UIManager uiManager, CancellationToken cancellationToken = default)
    {
        await uiManager.OpenMainRootAsync(UIType.MainUI, cancellationToken);


        //await uiManager.OpenVeryFrontRootAsync(원하는 UI타입, cancellationToken);
        // Main 레이어에 UI를 소환하고 싶을 때

        // await uiManager.OpenPopupRootAsync(원하는 UI타입, cancellationToken);
        // Popup 레이어에 UI를 소환하고 싶을 때

        //await uiManager.OpenVeryFrontRootAsync(원하는 UI타입, cancellationToken);
        // VeryFront 레이어에 UI를 소환하고 싶을 때

    }

    public static void CloseMain(this UIManager uiManager)
    {
        uiManager.Close(UIType.MainUI);
    }


    public static async UniTask OpenOverlayAsync(this UIManager uiManager, CancellationToken cancellationToken = default)
    {
        await uiManager.OpenVeryFrontRootAsync(UIType.OverlayUI, cancellationToken);
    }

    public static void CloseOverlay(this UIManager uiManager)
    {
        uiManager.Close(UIType.OverlayUI);
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

