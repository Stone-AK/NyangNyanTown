
public enum CatState
{
    None = 0,
    MoveToTarget,
    InBuildingAction,
    TargetMissing,
    SearchTarget,
    EscapeAction
}

public class CatViewModel : ViewModelBase
{
    private float _minSpeed = 1f;
    private float _maxSpeed = 5f;

    private float _catSpeed;
    private string _catId;
    private int _catBodyAddressableNum;
    private int _catEyeAddressableNum;
    private int _catMouthAddressableNum;
    private CatState _catState;

    public string CatId { get => _catId; }
    public int CatBodyAddressableNum { get => _catBodyAddressableNum; }
    public int CatEyeAddressableNum { get => _catEyeAddressableNum; }
    public int CatMouthAddressableNum { get => _catMouthAddressableNum; }

    public float CatSpeed
    {
        get => _catSpeed;
        set
        {
            if (_catSpeed != value)
            {
                _catSpeed = value;
                OnPropertyChanged(nameof(CatSpeed));
            }
        }
    }

    public CatState CatState 
    {
        get => _catState;
        set
        {
            if (_catState != value)
            {
                _catState = value;
                OnPropertyChanged(nameof(CatState));
            }
        }
    }

    public void InitRandomCatStat()
    {
        _catId = "Cat_Normal_01";
        _catState = CatState.MoveToTarget;
        _catSpeed = (float)(GameUtil.Random.NextDouble() * (_maxSpeed - _minSpeed) + _minSpeed);
        
        if(GameManager.Instance.DataManager.TryGetDataTable<CatBodySkinData>(out var bodySkinDataTable))
            _catBodyAddressableNum = GameUtil.Random.Next(bodySkinDataTable.Count);

        if (GameManager.Instance.DataManager.TryGetDataTable<CatEyeSkinData>(out var eyeSkinDataTable))
            _catEyeAddressableNum = GameUtil.Random.Next(eyeSkinDataTable.Count);

        if (GameManager.Instance.DataManager.TryGetDataTable<CatMouthSkinData>(out var mouthSkinDataTable))
            _catMouthAddressableNum = GameUtil.Random.Next(mouthSkinDataTable.Count);
    }

    public void InitSpecialCatStat(string catId)
    {
        _catId = catId;
        _catState = CatState.MoveToTarget;
        _catSpeed = (float)(GameUtil.Random.NextDouble() * (_maxSpeed - _minSpeed) + _minSpeed);

        _catBodyAddressableNum = 1000;
    }
}
