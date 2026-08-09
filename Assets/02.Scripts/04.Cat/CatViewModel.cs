using UnityEngine;


public enum CatState
{
    None = 0,
    TargetMove,
    Action,
    EscapeMove
}

public class CatViewModel : DNViewModelBase
{
    private float _minSpeed = 1f;
    private float _maxSpeed = 5f;

    // TODO(안우재/08.06) : 추후 데이터 부분 추가를 위해 대략적인 변수 선언
    private float _catSpeed;
    //private string _catMeshPrefabId;
    //private string _catMeterialId;
    //private string _catMeterialColorId;

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

    public void InitRandomCatStat()
    {
        _catSpeed = (float)(GameUtil.Random.NextDouble() * (_maxSpeed - _minSpeed) + _minSpeed);
    }
}
