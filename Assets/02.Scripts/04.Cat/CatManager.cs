using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class CatManager : BaseManager<CatManager>
{
    // TODO(안우재/08.06) : 추후 고양이 인구수가 할당되어야 함.
    // 게임 데이터의 고양이 인구수를 뷰모델의 데이터로 하여 변경 시 Poolling되는
    // 오브젝트 수 관리 및 소환 제한을 할 수 있도록 해야함.
    private int ActiveCatCount;
    public List<CatSpawner> CatSpanweList = new();

    public override UniTask InitializeAsync()
    {
        ActiveCatCount = 0;
        return UniTask.CompletedTask;
    }

    private CatViewModel InitRandomCatStat()
    {
        CatViewModel catViewModel = new CatViewModel();
        catViewModel.InitRandomCatStat();
        return catViewModel;
    }

    public async UniTask<GameObject> SpawnCat(Transform spawnTransform)
    {
        CatViewModel spawnCatVM = InitRandomCatStat();
        GameObject returnCatObj = await GameManager.Instance.ObjectManager.SpawnAsync("Prefab/Cat_Prefab", this.gameObject.transform, spawnTransform);

        if (returnCatObj == null)
            return null;

        if(returnCatObj.TryGetComponent<CatView>(out var catView))
            catView.InitCatView(spawnCatVM);

        ActiveCatCount++;
        return returnCatObj;
    }

    public void DspawnCat(GameObject targetDspawnObject)
    {
        if(targetDspawnObject == null) 
            return;

        ActiveCatCount--;
        GameManager.Instance.ObjectManager.Despawn(targetDspawnObject);
    }

    public bool IsCatSpawnAvailable()
    {
        // TODO(안우재/08.12) : 소환된 고양이와 인구수 비교해서 생성 여부 판단 코드 추가필요
        // GameManager에 EconomyManager와 연관되는 부분 생성 시 그부분과 연계
        // GameManager.Instance.EconomyService 등과 같은거 이용
        

        return true;
    }
}
