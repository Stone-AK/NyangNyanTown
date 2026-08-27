using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class UIManager : BaseManager<UIManager>
{
    private readonly HashSet<UIType> _loadingUISet = new HashSet<UIType>();
    private readonly HashSet<UIType> _openedUISet = new HashSet<UIType>();

    private readonly Dictionary<UIRootType, RectTransform> _rootCacheDictionary = new Dictionary<UIRootType, RectTransform>();
    private readonly Dictionary<UIType, BaseUI> _createdUICacheDictionary = new Dictionary<UIType, BaseUI>();

    private UILayer _uiLayer;

    public override async UniTask InitializeAsync()
    {
        if (_uiLayer == null)
        {
            await CreateLayer();
            CacheRootDictionary();
        }
    }

    public async UniTask<BaseUI> OpenMainRootAsync(UIType uiType, CancellationToken cancellationToken = default)
    {
        BaseUI baseUI = await OpenUIAsync(uiType, UIRootType.Main, cancellationToken);
        return baseUI;
    }

    // 메인
    public async UniTask<BaseUI> OpenPopupRootAsync(UIType uiType, CancellationToken cancellationToken = default)
    {
        return await OpenUIAsync(uiType, UIRootType.Popup, cancellationToken);
    }

    // 팝업
    public async UniTask<BaseUI> OpenVeryFrontRootAsync(UIType uiType, CancellationToken cancellationToken = default)
    {
        return await OpenUIAsync(uiType, UIRootType.VeryFront, cancellationToken);
    }

    

    public void Close(UIType uiType)
    {
        CloseUI(uiType);
    }

    private void CacheRootDictionary()
    {
        _rootCacheDictionary.Clear();

        CacheUIRoot(UIRootType.Main, _uiLayer.Main);
        CacheUIRoot(UIRootType.Popup, _uiLayer.Popup);
        CacheUIRoot(UIRootType.VeryFront, _uiLayer.VeryFront);
    }

    private async UniTask<BaseUI> OpenUIAsync(UIType uiType, UIRootType uiRoot, CancellationToken cancellationToken)
    {
        if (!TryGetRootRectTransform(uiRoot, out RectTransform rootRectTransform))
        {
            Debug.LogError($"[{nameof(UIManager)}:{nameof(OpenUIAsync)}] '{uiRoot}'에 해당하는 Root RectTransform을 찾을 수 없습니다.");
            return null;
        }


        if (_loadingUISet.Contains(uiType))
        {
            Debug.LogWarning($"[{nameof(UIManager)}:{nameof(OpenUIAsync)}] '{uiType}' UI가 이미 로딩 중입니다.");
            return null;
        }

        if (_openedUISet.Contains(uiType))
        {
            BaseUI cachedUI = _createdUICacheDictionary[uiType];
            return cachedUI;
        }

        _loadingUISet.Add(uiType);

        try
        {
            if (_createdUICacheDictionary.TryGetValue(uiType, out BaseUI cachedUI))
            {
                SetCreatedUIRoot(cachedUI, rootRectTransform);
                OpenUI(uiType, cachedUI);
                return cachedUI;
            }

            BaseUI createdUI = await CreateUIAsync(uiType, rootRectTransform, cancellationToken);

            if (createdUI == null)
            {
                Debug.LogError($"[{nameof(UIManager)}:{nameof(OpenUIAsync)}] '{uiType}' UI 생성에 실패하여 UI를 열 수 없습니다.");
                return null;
            }

            _createdUICacheDictionary[uiType] = createdUI;

            OpenUI(uiType, createdUI);
            return createdUI;
        }
        finally
        {
            _loadingUISet.Remove(uiType);
        }
    }

    private void OpenUI(UIType uiType, BaseUI uiObject)
    {
        _openedUISet.Add(uiType);
        uiObject.gameObject.SetActive(true);
    }

    private async UniTask<BaseUI> CreateUIAsync(UIType uiType, RectTransform rectTransform, CancellationToken cancellationToken)
    {
        string addressableKey = AddressableKey.GetUIKey(uiType);
        GameObject uiPrefab = await GameManager.Instance.ResourceManager.LoadAssetAsync<GameObject>(addressableKey, cancellationToken);

        if (uiPrefab == null)
        {
            Debug.LogError($"[{nameof(UIManager)}:{nameof(CreateUIAsync)}] '{uiType}' UI 프리팹을 로드하지 못했습니다.");
            return null;
        }

        if (!uiPrefab.TryGetComponent(out BaseUI uibase))
        {
            Debug.LogError($"[{nameof(UIManager)}:{nameof(CreateUIAsync)}] '{uiType}' UI 프리팹에서 UIBase 컴포넌트를 찾을 수 없습니다.");
            return null;
        }

        BaseUI uiInstance = Instantiate(uibase, rectTransform);
        return uiInstance;
    }

    private void CloseUI(UIType uiType)
    {
        if (!_openedUISet.Contains(uiType))
        {
            Debug.LogWarning($"[{nameof(UIManager)}:{nameof(CloseUI)}] '{uiType}' UI가 열려 있지 않습니다.");
            return;
        }

        BaseUI uiObject = _createdUICacheDictionary[uiType];

        _openedUISet.Remove(uiType);
        uiObject.gameObject.SetActive(false);
    }

    private bool TryGetRootRectTransform(UIRootType uiRoot, out RectTransform rectTransform)
    {
        bool hasRootRectTransform = _rootCacheDictionary.TryGetValue(uiRoot, out rectTransform);
        return hasRootRectTransform;
    }

    private async UniTask CreateLayer()
    {
        GameObject uiLayerPrefab = await GameManager.Instance.ResourceManager.LoadAssetAsync<GameObject>(AddressableKey.GetPrefabKey(PrefabType.UILayer), destroyCancellationToken);

        if (uiLayerPrefab == null)
        {
            Debug.LogError($"[{nameof(UIManager)}:{nameof(CreateLayer)}] UILayer 프리팹을 로드하지 못했습니다.");
            return;
        }

        if (!uiLayerPrefab.TryGetComponent(out UILayer uiLayer))
        {
            Debug.LogError($"[{nameof(UIManager)}:{nameof(CreateLayer)}] UILayer 프리팹에 UILayer 컴포넌트가 없습니다.");
            return;
        }

        _uiLayer = Instantiate(uiLayer);
        _uiLayer.name = uiLayer.name;
    }

    private void CacheUIRoot(UIRootType uiRoot, RectTransform rectTransform)
    {
        if (!_rootCacheDictionary.TryAdd(uiRoot, rectTransform))
        {
            Debug.LogError($"[{nameof(UIManager)}:{nameof(CacheUIRoot)}] UIRoot '{uiRoot}'가 이미 루트 캐시에 등록되어 있습니다.");
        }
    }

    private void SetCreatedUIRoot(BaseUI uiObject, RectTransform rectTransform)
    {
        if (!uiObject.transform.IsChildOf(rectTransform))
        {
            uiObject.transform.SetParent(rectTransform);
        }
    }
    public async UniTask OpenBuildingPopupAsync(Building building,CancellationToken cancellationToken = default)
    {
        BaseUI baseUI = await OpenPopupRootAsync(UIType.BuildingPopUpUI, cancellationToken);

        if (baseUI is BuildingPopUpUIView popup)
        {
            popup.Initialize(building);
        }
    }
    public async UniTask OpenBuildConfirmPopUpUIAsync(BuildingData data, CancellationToken cancellationToken = default)
    {
        BaseUI baseUI = await OpenPopupRootAsync(UIType.BuildConfirmPopUpUI, cancellationToken);

        if (baseUI is BuildConfirmPopUpUIView popup)
        {
            BuildConfirmPopUpUIViewModel vm = new BuildConfirmPopUpUIViewModel(data);
            popup.Init(vm);
        }
    }

    public async UniTask OpenCatInfoPopupAsync(CatView chooseCat, CancellationToken cancellationToken = default)
    {
        BaseUI baseUI = await OpenPopupRootAsync(
        UIType.OnClickCatInfoPopUp,
        cancellationToken);

        if (baseUI is OnClickCatInfoPopUp popup)
        {
            popup.SettingPopUp(chooseCat);
        }

    }
}