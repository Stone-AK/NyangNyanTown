using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : BaseManager<GameManager>
{
    public static GameManager Instance { get; set; }
    public ResourceManager ResourceManager { get; private set; }
    public GameDataManager DataManager { get; private set; }
    public UIManager UIManager { get; private set; }
    public ObjectManager ObjectManager { get; private set; }

    


    public CurrencyService CurrencyService { get; private set; }


    private void Awake()
    {
        EnsureSingleton();
        SetupManagers();

        InitService();
    }

    private void InitService()
    {
        CurrencyService = new CurrencyService();
    }



    
    public async UniTask StartGame()
    {
        await InitializeManagersAsync();

        await GameManager.Instance.UIManager.OpenMainUIAsync();
    }

    public async UniTask InitializeManagersAsync()
    {
        await InitializeAsync();
        await ResourceManager.InitializeAsync();
        await DataManager.InitializeAsync();
        await ObjectManager.InitializeAsync();
        await UIManager.InitializeAsync();

    }
    public override UniTask InitializeAsync()
    {
        return UniTask.CompletedTask;
    }
    private void EnsureSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"[{nameof(GameManager)}:{nameof(EnsureSingleton)}] 중복된 인스턴스가 발견되어 {gameObject.name} 오브젝트를 파괴합니다.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void SetupManagers()
    {
        ResourceManager = this.GetComponent<ResourceManager>();
        DataManager = this.GetComponent<GameDataManager>();
        UIManager = this.GetComponent<UIManager>();
        ObjectManager = this.GetComponent<ObjectManager>();
      
    }
}
