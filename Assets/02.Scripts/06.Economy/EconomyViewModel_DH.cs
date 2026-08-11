public class EconomyViewModel_DH : ViewModelBase
{
    private int _catCount;
    private int _currentGold;
    private int _specialCatCount;

    public int CatCount
    {
        get => _catCount;
        set
        {
            if (_catCount != value) // 값이 진짜 변했다면
            {
                _catCount = value;
                OnPropertyChanged(nameof(CatCount)); // 변했다고 알림
            }
        }
    }

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

}
