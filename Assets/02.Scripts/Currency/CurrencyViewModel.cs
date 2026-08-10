using UnityEngine;

public class CurrencyViewModel : ViewModelBase
{

    public void InvokeOnceOnInit()
    {
        OnPropertyChanged(nameof(Gold));
        OnPropertyChanged(nameof(Fish));
        OnPropertyChanged(nameof(CatCurrentCount));
        OnPropertyChanged(nameof(CatMaxCount));
        OnPropertyChanged(nameof(BuildingCount));
    }

    private int _gold;
    public int Gold
    {
        get => _gold;
        set
        {
            if (_gold != value)
            {
                _gold = value;
                OnPropertyChanged(nameof(Gold));
            }
        }
    }

    private int _fish;
    public int Fish
    {
        get => _fish;
        set
        {
            if (_fish != value)
            {
                _fish = value;
                OnPropertyChanged(nameof(Fish));
            }
        }
    }

    private int _catCurrentCount;
    public int CatCurrentCount
    {
        get => _catCurrentCount;
        set
        {
            if (_catCurrentCount != value)
            {
                _catCurrentCount = value;
                OnPropertyChanged(nameof(CatCurrentCount));
            }
        }
    }

    private int _catMaxCount;
    public int CatMaxCount
    {
        get => _catMaxCount;
        set
        {
            if (_catMaxCount != value)
            {
                _catMaxCount = value;
                OnPropertyChanged(nameof(CatMaxCount));
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
