using UnityEngine;


public enum CatState
{
    None = 0,
    TargetMove,
    InBuildingAction,
    EscapeMove,
    EscapeAction
}

public class CatViewModel : ViewModelBase
{
    private float _minSpeed = 1f;
    private float _maxSpeed = 5f;

    // TODO(안우재/08.06) : 추후 데이터 부분 추가를 위해 대략적인 변수 선언
    private float _catSpeed;
    //private string _catMeshPrefabId;
    //private string _catMeterialId;
    //private string _catMeterialColorId;
    private CatState _catState;

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
        _catState = CatState.TargetMove;
        _catSpeed = (float)(GameUtil.Random.NextDouble() * (_maxSpeed - _minSpeed) + _minSpeed);
    }
}
