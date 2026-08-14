using System.Collections.Generic;

public class CatEncyclopediaViewModel : ViewModelBase
{
    public string CatInfoDataId;
    private bool _isCollected = false;

    public bool IsCollected
    {
        get => _isCollected;
        set
        {
            if (_isCollected != value)
            {
                _isCollected = value;
                OnPropertyChanged(nameof(IsCollected));
            }
        }
    }
}


