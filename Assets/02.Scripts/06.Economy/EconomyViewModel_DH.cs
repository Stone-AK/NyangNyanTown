public class EconomyViewModel_DH : ViewModelBase
{


    public void InvokeOnceOnInit()
    {
        OnPropertyChanged(nameof(CatCurrentCount));
        OnPropertyChanged(nameof(CurrentGold));
        OnPropertyChanged(nameof(CurrentFish));
        OnPropertyChanged(nameof(SpecialCatCount));
        OnPropertyChanged(nameof(BuildingCount));
    }

    private int _catCurrentCount;

    public int CatCurrentCount
    {
        get => _catCurrentCount;
        set
        {
            if (_catCurrentCount != value) // 값이 진짜 변했다면
            {
                _catCurrentCount = value;
                OnPropertyChanged(nameof(CatCurrentCount)); // 변했다고 알림
            }
        }
    }

    
    private int _currentGold;

    public int CurrentGold
    {
        get => _currentGold;
        set
        {
            if (_currentGold != value)
            {
                _currentGold = value;
                OnPropertyChanged(nameof(CurrentGold));
            }
        }
    }

    private int _currentfish;
    public int CurrentFish
    {
        get => _currentfish;
        set
        {
            if (_currentfish != value)
            {
                _currentfish = value;
                OnPropertyChanged(nameof(CurrentFish));
            }
        }
    }

    private int _specialCatCount;
    public int SpecialCatCount
    {
        get => _specialCatCount;
        set
        {
            if (_specialCatCount != value)
            {
                _specialCatCount = value;
                OnPropertyChanged(nameof(SpecialCatCount));
            }
        }
    }

    private int _buildingCount;
    public int BuildingCount
    {
        get => _buildingCount;
        set
        {
            if (_buildingCount != value)
            {
                _buildingCount = value;
                OnPropertyChanged(nameof(BuildingCount));
            }
        }
    }

}
