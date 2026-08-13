using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : BaseManager<GameManager>
{
    public static GameManager Instance { get; set; }
    public ResourceManager ResourceManager { get; private set; }
    public GameDataManager DataManager { get; private set; }
    public UIManager UIManager { get; private set; }
    public ObjectManager ObjectManager { get; private set; }
    public BuildManager BuildManager { get; private set; }
    public MapManager MapManager { get; private set; }
    public CatManager CatManager { get; private set; }




    public EconomyService_DH EconomyService_DH { get; private set; }



    [SerializeField] private GameObject WJPrefab;
    [SerializeField] private GameObject DHPrefab;
    [SerializeField] private GameObject JDPrefab;
    [SerializeField] private GameObject GHPrefab;
    //임시로 등록. 나중에 어드레서블로 변경해야 함


    private void Awake()
    {
        EnsureSingleton();
        SetupManagers();
        InitService();
    }

    private void InitService()
    {
        //서비스를 등록하려면 여기에 추가!
        EconomyService_DH = new EconomyService_DH();
    }



    
    public async UniTask StartGame()
    {
        await InitializeManagersAsync();

        await GameManager.Instance.UIManager.OpenMainUIAsync();

        if (WJPrefab != null) {
            WJPrefab.SetActive(true);
            DHPrefab.SetActive(true);
            JDPrefab.SetActive(true);
            GHPrefab.SetActive(true);
        }
        
    }

    public async UniTask InitializeManagersAsync()
    {
        await InitializeAsync();
        await ResourceManager.InitializeAsync();
        await DataManager.InitializeAsync();
        await ObjectManager.InitializeAsync();
        await UIManager.InitializeAsync();
        await BuildManager.InitializeAsync();
        await MapManager.InitializeAsync();
        await CatManager.InitializeAsync();
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
        BuildManager = this.GetComponent<BuildManager>(); 
        MapManager = this.GetComponent<MapManager>();
        CatManager = this.GetComponent<CatManager>();
    }
}
