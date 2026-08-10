using UnityEngine;

public class GameViewModel : ViewModelBase
{
    public void InvokeInitProperty()
    {
        OnPropertyChanged(nameof(HadCredit));
    }

    private int _hadCredit;
    public int HadCredit
    {
        get => _hadCredit;
        set
        {
            if (_hadCredit != value)
            {
                _hadCredit = value;
                OnPropertyChanged(nameof(HadCredit));
            }
        }
    }
}
