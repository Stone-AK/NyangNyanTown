using UnityEngine;
using UnityEngine.UI;


public class EconomyManager_DH : MonoBehaviour
// 고양이 수의 체크는 건물이 늘고 줄때 계산식 돌려서 체크, 현재는 버튼을 하나 만들어서 고양이 수를 강제적으로 늘리는 방식을 써보자
// 골드 수 체크는 일정 간격으로 돌아가는 업데이트로 고양이 수에 계산식 돌려서 체크
// 계산식은 나중에 static 클래스로 관리
{
    [Header("Economy Calculation Settings")]
    [SerializeField] private int _incomeGoldBase = 10;
    [SerializeField] private int _specialCatCount = 0;
    [SerializeField] private float _specialCatsMultiply = 0.1f;
    [SerializeField] private float _goldInterval = 2.0f;

    [Header("UI References")]
    [SerializeField] private EconomyHUDView_DH View_CatHUD; // 뷰모델과 연결된 뷰
    [SerializeField] private Button Button_AddCat; // 테스트용 버튼, 고양이 늘리는데 사용
    [SerializeField] private Button Button_AddSpecialCat; // 테스트용 버튼, 건물 늘리는데 사용
    // 버튼들 방식을 나중에 2D 프로젝트때 처럼 바꿀 필요성이 있음

    private EconomyViewModel_DH _catViewModel; // 뷰모델 선언
    public EconomyViewModel_DH CatViewModel => _catViewModel; // 접근 가능하도록 프로퍼티 선언

    private float _timer = 0.0f;



    public void Start()
    {
        if (View_CatHUD == null)
        {
            Debug.LogError("[CatManager_DH] View_CatHUD가 Inspector에 연결되지 않았습니다.");
            return;
        }

        _catViewModel = new EconomyViewModel_DH(); // 뷰모델 초기화
        View_CatHUD.BindViewModel(_catViewModel); // 뷰모델과 뷰를 바인딩

        if (Button_AddCat != null) Button_AddCat.onClick.AddListener(OnClickAddCat);
        else
        {
            Debug.LogError("[CatManager_DH] Button_AddCat가 Inspector에 연결되지 않았습니다.");
        }

        if (Button_AddSpecialCat != null) Button_AddSpecialCat.onClick.AddListener(OnClickAddSpecialCat);
        else
        {
            Debug.LogError("[CatManager_DH] Button_AddSpecialCat이 Inspector에 연결되지 않았습니다.");
        }
    }

    public void Update()
    {
        if (_catViewModel == null) return;

        _timer += Time.deltaTime;

        if (_timer >= _goldInterval) // UI를 직접 수정하는 것이 아닌, 뷰모델 데이터만 변경
        {
            _timer -= _goldInterval;
            int addGold = GameUtil.CalcEconomyGold(_catViewModel.CatCount, _incomeGoldBase, _catViewModel.SpecialCatCount, _specialCatsMultiply);
            _catViewModel.CurrentGold += addGold;
        }
    }

    private void OnClickAddCat()
    {
                if (_catViewModel == null) return;
        _catViewModel.CatCount++;
    }

    private void OnClickAddSpecialCat()
    {
        if (_catViewModel == null) return;
        _catViewModel.SpecialCatCount++;
    }

}