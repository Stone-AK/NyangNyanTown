//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;


//public class Test_EconomyManager : MonoBehaviour
//// 고양이 수의 체크는 건물이 늘고 줄때 계산식 돌려서 체크, 현재는 버튼을 하나 만들어서 고양이 수를 강제적으로 늘리는 방식을 써보자
//// 골드 수 체크는 일정 간격으로 돌아가는 업데이트로 고양이 수에 계산식 돌려서 체크
//// 계산식은 나중에 static 클래스로 관리
//{
//    [Header("Cat Settings")]
//    [SerializeField] private int _goldPerCat = 10;
//    [SerializeField] private float _goldInterval = 3.0f;

//    [Header("UI References")]
//    [SerializeField] private TextMeshProUGUI Text_CatCount;
//    [SerializeField] private TextMeshProUGUI Text_GoldCount;
//    [SerializeField] private TextMeshProUGUI Text_BuildingCount;
//    [SerializeField] private Button Button_AddCat; // 테스트용 버튼, 고양이 늘리는데 사용
//    [SerializeField] private Button Button_AddBuilding; // 테스트용 버튼, 건물 늘리는데 사용


//    private int _catCount = 0;
//    private int _currentGold = 0;
//    private int _buildingCount = 0;
//    private float _timer = 0.0f;


//    public void Start()
//    {
//        if (Button_AddCat == null || Text_CatCount == null || Text_GoldCount == null || Text_BuildingCount == null)
//        {
//            Debug.LogError("[Test_CatManager] Inspector에 UI 컴포넌트가 연결되지 않았습니다.");
//            return;
//        }

//        Button_AddCat.onClick.AddListener(OnClickAddCat);
//        Button_AddBuilding.onClick.AddListener(OnClickAddBuilding);

//        UpdateUI();
//    }

//    public void Update()
//    {
//        _timer += Time.deltaTime;

//        if (_timer >= _goldInterval)
//        {
//            _timer -= _goldInterval;
//            _currentGold += _catCount * _goldPerCat;

//            UpdateUI();
//        }
//    }


//    private void OnClickAddCat()
//    {
//        _catCount++;
//        UpdateUI();
//    }

//    private void OnClickAddBuilding()
//    {
//        // 건물 추가 시 고양이 수 증가 로직
//        _buildingCount++;
//        _catCount += 10;
//        UpdateUI();
//    }


//    private void UpdateUI()
//    {
//        if (Text_CatCount != null)
//        {
//            Text_CatCount.text = $"Cats: {_catCount}";
//        }

//        if (Text_GoldCount != null)
//        {
//            Text_GoldCount.text = $"Nyan: {_currentGold}";
//        }

//        if (Text_BuildingCount != null)
//        {
//            Text_BuildingCount.text = $"Building: {_buildingCount}";
//        }
//    }
//}