using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class CatManager : BaseManager<CatManager>
{
    private string _commonCatId;
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
    private void ChangedCatSpawnWeight()
    {
        if(_catSpawnWeightList.Count == 0)
        {
            if (GameManager.Instance.DataManager.TryGetDataTable<CatInfoData>(out var catDataTable))
            {
                int tmpInt = 0;
                foreach (var catData in catDataTable)
                {
                    if (tmpInt == 0)
                    {
                        _commonCatId = catData.Key;
                        tmpInt++;
                    }
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

        if (spawnedCatId == _commonCatId)
        {
            catViewModel.InitRandomCatStat();
            return catViewModel;
        }

        catViewModel.InitSpecialCatStat(spawnedCatId);

        return catViewModel;
    }

    public string SelectRandomCatIdByWeight()
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

        ChangedCatSpawnWeight();

        return true;
    }

    public async UniTask<GameObject> SpawnCat(Vector3 spawnPosition)
    {
        if (!IsCatSpawnAvailable())
            return null;

        Building targetBuilding = FindAvailableTargetBuilding();

        if (targetBuilding == null)
            return null;

        string spawnedCatId = SelectRandomCatIdByWeight();

        if (spawnedCatId == null)
        {
            Debug.LogError("고양이 추첨 방식에 심각한 로직 실패 발생. 즉시 CatManager의 SelectRandomCatIdByWeight()메서드 수정 필요");
            return null;
        }

        if(spawnedCatId != _commonCatId)
        {
            GameManager.Instance.EconomyService_DH.AddCurrentFish(1);
        }
        
        CatViewModel spawnCatVM = InitCatStat(spawnedCatId);
        _activeCatCount++;

        CatEncyclopediaViewModel spawnCatEncycloiediaVM = GameManager.Instance.EconomyService_DH.CatEncyclopediaList[spawnedCatId];

        GameObject returnCatObj = await GameManager.Instance.ObjectManager.SpawnAsync("Prefab/Cat_Prefab", this.gameObject.transform, spawnPosition, Quaternion.identity);

        if (returnCatObj == null)
        {
            _activeCatCount--;
            return null;
        }

        if (returnCatObj.TryGetComponent<CatView>(out var catView))
        {
            catView.InitCatView(spawnCatVM, targetBuilding, spawnCatEncycloiediaVM);
        }

        return returnCatObj;
    }

    public Building FindAvailableTargetBuilding()
    {
        Building candidateTargetBuilding = null;
        float highestAvailableSpaceRate = 0f;
        int sameRateBuildingCount = 0;

        foreach (var placeBuildingData in GameManager.Instance.MapManager._currentBuildingLDic)
        {
            Vector3 searchPosition = new Vector3(placeBuildingData.Value.RootX, 0,GameManager.Instance.BuildManager.BuildingZ);

            Collider[] colliders = Physics.OverlapSphere(searchPosition, 0.01f);

            foreach (var collider in colliders)
            {
                if (collider == null)
                    continue;

                Building building = collider.GetComponentInParent<Building>();

                if (building == null)
                    continue;

                if (building.GetComponent<CatSpawner>() != null)
                    continue;

                if (building.GetAvailableCatPointCount() == 0)
                    continue;

                float availableSpaceRate = building.GetAvailableSpaceRate();

                if (highestAvailableSpaceRate < availableSpaceRate)
                {
                    highestAvailableSpaceRate = availableSpaceRate;
                    candidateTargetBuilding = building;
                    sameRateBuildingCount = 1;
                }
                else if (Mathf.Approximately(highestAvailableSpaceRate, availableSpaceRate))
                {
                    sameRateBuildingCount++;
                    if (GameUtil.Random.Next(sameRateBuildingCount) == 0)
                    {
                        candidateTargetBuilding = building;
                    }
                }
            }
        }

        return candidateTargetBuilding;
    }

    public void DespawnCat(GameObject targetDspawnObject)
    {
        if(targetDspawnObject == null) 
            return;

        _activeCatCount--;
        GameManager.Instance.ObjectManager.Despawn(targetDspawnObject);
    }

    public void DespawnAllCats()
    {
        CatView[] activeCats = FindObjectsByType<CatView>(FindObjectsSortMode.None);

        foreach (CatView catView in activeCats)
        {
            if (catView != null)
            {
                catView.DespawnImmediately();
            }
        }

        _activeCatCount = 0;
    }

    public bool IsCatSpawnAvailable()
    {
        if(_activeCatCount < GameManager.Instance.EconomyService_DH.GetEconomyViewModel().CatCurrentCount)
            return true;

        return false;
    }
}
