using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class CatManager : BaseManager<CatManager>
{
    private int _activeCatCount;
    public List<CatSpawner> CatSpanweList = new();
    private Dictionary<string, int> _catSpawnWeightList = new();
    private int _totalWeight = 0;
    private List<CatBodySkinData> _catBodyDataList = new();
    private List<CatEyeSkinData> _catEyeDataList = new();
    private List<CatMouthSkinData> _catMouthDataList = new();
    public List<CatBodySkinData> CatBodySkinDatas {get => _catBodyDataList;}
    public List<CatEyeSkinData> CatEyeSkinDatas {get => _catEyeDataList; }
    public List<CatMouthSkinData> CatMouthSkinDatas {get => _catMouthDataList; }

    public override UniTask InitializeAsync()
    {
        _activeCatCount = 0;
        ChangedCatSpawnWeight();
        InitCatMaterialList();
        return UniTask.CompletedTask;
    }

    private void InitCatMaterialList()
    {
        if (GameManager.Instance.DataManager.TryGetDataTable<CatBodySkinData>(out var bodySkinDataTable))
        {
            foreach (var addData in bodySkinDataTable.Values)
            {
                _catBodyDataList.Add(addData);
            }
        }

        if (GameManager.Instance.DataManager.TryGetDataTable<CatEyeSkinData>(out var eyeSkinDataTable))
        {
            foreach (var addData in eyeSkinDataTable.Values)
            {
                _catEyeDataList.Add(addData);
            }
        }

        if (GameManager.Instance.DataManager.TryGetDataTable<CatMouthSkinData>(out var mouthSkinDataTable))
        {
            foreach (var addData in mouthSkinDataTable.Values)
            {
                _catMouthDataList.Add(addData);
            }
        }
    }

    // 나중에 가중치 값 변경 시 호출 되어야함
    public void ChangedCatSpawnWeight()
    {
        if(_catSpawnWeightList.Count == 0)
        {
            if (GameManager.Instance.DataManager.TryGetDataTable<CatInfoData>(out var catDataTable))
            {
                foreach (var catData in catDataTable)
                {
                    _catSpawnWeightList.Add(catData.Key, catData.Value.CatAppearanceWeight);
                }
            }
        }

        _totalWeight = 0;
        foreach (var catWeight in _catSpawnWeightList.Values)
        {
            _totalWeight += catWeight;
        }
    }

    private CatViewModel InitCatStat(string spawnedCatId)
    {
        if(spawnedCatId == null)
            return null;

        CatViewModel catViewModel = new CatViewModel();

        if (spawnedCatId == "Cat_Normal_01")
        {
            catViewModel.InitRandomCatStat();
            return catViewModel;
        }

        catViewModel.InitSpecialCatStat(spawnedCatId);

        return catViewModel;
    }

    private string SelectRandomCatIdByWeight()
    {
        if (_catSpawnWeightList == null)
            return null;

        if(_totalWeight <= 0) 
            return null;

        int selectedNum = GameUtil.Random.Next(_totalWeight);
        int cumulative = 0;

        foreach(var catWeightData in _catSpawnWeightList)
        {
            cumulative += catWeightData.Value;

            if(selectedNum < cumulative)
                return catWeightData.Key;
        }

        return null;
    }

    public bool TryChangeSpawnWeight(string changedCatId, int upDownValue)
    {
        if (!_catSpawnWeightList.TryGetValue(changedCatId, out int currentWeight))
        {
            Debug.LogWarning($"가중치를 변경할 고양이가 없습니다. CatId: {changedCatId}");
            return false;
        }

        int newWeight = Mathf.Max(0, currentWeight + upDownValue);

        _catSpawnWeightList[changedCatId] = newWeight;
        _totalWeight += newWeight - currentWeight;

        return true;
    }

    public async UniTask<GameObject> SpawnCat(Transform spawnTransform)
    {
        if (!IsCatSpawnAvailable())
            return null;

        string spawnedCatId = SelectRandomCatIdByWeight();

        if (spawnedCatId == null)
        {
            Debug.LogError("고양이 추첨 방식에 심각한 로직 실패 발생. 즉시 CatManager의 SelectRandomCatIdByWeight()메서드 수정 필요");
            return null;
        }

        CatViewModel spawnCatVM = InitCatStat(spawnedCatId);

        _activeCatCount++;

        GameObject returnCatObj = await GameManager.Instance.ObjectManager.SpawnAsync("Prefab/Cat_Prefab", this.gameObject.transform, spawnTransform);

        if (returnCatObj == null)
        {
            _activeCatCount--;
            return null;
        }

        if (returnCatObj.TryGetComponent<CatView>(out var catView))
            catView.InitCatView(spawnCatVM);

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
