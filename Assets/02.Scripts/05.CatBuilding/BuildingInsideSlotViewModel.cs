
public class BuildingInsideSlotViewModel : ViewModelBase
{
    private bool _isSlotFilled = false;

    public bool IsSlotFilled
    {
        get => _isSlotFilled;
        set
        {
            if (_isSlotFilled != value)
            {
                _isSlotFilled = value;
                OnPropertyChanged(nameof(IsSlotFilled));
            }
        }
    }

    public void InvokeInitProperty()
    {
        OnPropertyChanged(nameof(IsSlotFilled));
    }
}
