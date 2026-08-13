using System.Collections.Generic;
using UnityEngine;

public class CatEncyclopediaDataClass
{
    public CatInfoData CatInfoData { get; set; }
    public bool IsCollected = false;
}

public class CatEncyclopediaViewModel : ViewModelBase
{
    // TODO(안우재/08.12) : 데이터 초기화 시 CatInfoData는 DataManager에서 가져오고
    // IsCollected는 동적 데이터로 로드 매니저로부터 가져와야함. 기본적으로는 false 할당
    private List<CatEncyclopediaDataClass> _catEncyclopediaList = new();

    // TODO(안우재/08.12) : 데이터 초기화 시 CatInfoData는 DataManager에서 가져오고
}
