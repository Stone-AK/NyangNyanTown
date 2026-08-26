public class EconomyViewModel_DH : ViewModelBase
{


    public void InvokeOnceOnInit()
    {
        OnPropertyChanged(nameof(CatCurrentCount));
        OnPropertyChanged(nameof(CurrentGold));
        OnPropertyChanged(nameof(CurrentFish));
        OnPropertyChanged(nameof(SpecialCatAdd));
        OnPropertyChanged(nameof(SpecialCatMultiply));
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

    private int _currentFish;
    public int CurrentFish
    {
        get => _currentFish;
        set
        {
            if (_currentFish != value)
            {
                _currentFish = value;
                OnPropertyChanged(nameof(CurrentFish));
            }
        }
    }

    private int _specialCatAdd;
    public int SpecialCatAdd
    {
        get => _specialCatAdd;
        set
        {
            if (_specialCatAdd != value)
            {
                _specialCatAdd = value;
                OnPropertyChanged(nameof(SpecialCatAdd));
            }
        }
    }

    private float _specialCatsMultiply;
    public float SpecialCatMultiply
    {
        get => _specialCatsMultiply;
        set
        {
            if (_specialCatsMultiply != value)
            {
                _specialCatsMultiply = value;
                OnPropertyChanged(nameof(SpecialCatMultiply));
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
