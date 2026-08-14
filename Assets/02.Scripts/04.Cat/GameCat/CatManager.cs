using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class CatManager : BaseManager<CatManager>
{
    private int _activeCatCount;
    public List<CatSpawner> CatSpanweList = new();

    public override UniTask InitializeAsync()
    {
        _activeCatCount = 0;
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

        _activeCatCount++;
        return returnCatObj;
    }

    public void DespawnCat(GameObject targetDspawnObject)
    {
        if(targetDspawnObject == null) 
            return;

        _activeCatCount--;
        GameManager.Instance.ObjectManager.Despawn(targetDspawnObject);
    }

    public bool IsCatSpawnAvailable()
    {
        if(_activeCatCount < GameManager.Instance.EconomyService_DH.GetEconomyViewModel().CatCurrentCount)
            return true;

        return false;
    }
}
