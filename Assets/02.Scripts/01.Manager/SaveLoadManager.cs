using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class SaveLoadManager : BaseManager<SaveLoadManager>
{
    private SaveDataModel _saveData = new();

    public override UniTask InitializeAsync()
    {
        return UniTask.CompletedTask;
    }

    public void SaveGameData()
    {
        _saveData.Gold = GameManager.Instance.EconomyService_DH.GetEconomyViewModel().CurrentGold;
        _saveData.Fish = GameManager.Instance.EconomyService_DH.GetEconomyViewModel().CurrentFish;
        SaveCollectedSpecialCatList();
        _saveData.LandLevel = GameManager.Instance.MapManager._lvm.LandLevel;
        SavePlacedBuildingList();

        string json = JsonConvert.SerializeObject(_saveData, Formatting.Indented);

        string savePath = Path.Combine(Application.persistentDataPath, "SaveData.json");

        File.WriteAllText(savePath, json);

        GameManager.Instance.UIManager.OpenSaveLoadCompletePopupAsync(SaveLoadPopupType.Save, true).Forget();
        Debug.Log($"저장 완료: {savePath}");
    }

    public async UniTask LoadGameData()
    {
        LoadCollectedCatData();
        LoadGold();
        LoadFish();
        LoadLandInfo();
        await LoadPlaceBuilding();

        await GameManager.Instance.UIManager.OpenSaveLoadCompletePopupAsync(SaveLoadPopupType.Load, true);
    }

    public bool TryReadGameData()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "SaveData.json");

        if (File.Exists(savePath) == false)
        {
            GameManager.Instance.UIManager.OpenSaveLoadCompletePopupAsync(SaveLoadPopupType.Read, false).Forget();
            Debug.Log("저장 파일이 존재하지 않습니다.");
            return false;
        }

        string json = File.ReadAllText(savePath);
        SaveDataModel loadData = JsonConvert.DeserializeObject<SaveDataModel>(json);

        if (loadData == null)
        {
            Debug.LogError("저장 파일을 SaveDataModel로 변환하지 못했습니다.");
            return false;
        }

        _saveData = loadData;
        Debug.Log($"로드 완료: {savePath}");
        return true;
    }

    private void LoadCollectedCatData()
    {
        EconomyService_DH economyService = GameManager.Instance.EconomyService_DH;
        var catEncyclopediaList = economyService.CatEncyclopediaList;

        foreach (var catViewModel in catEncyclopediaList.Values)
        {
            if (catViewModel != null)
                catViewModel.IsCollected = false;
        }

        if (_saveData.CollectedCatIdList != null)
        {
            foreach (string collectedCatId in _saveData.CollectedCatIdList)
            {
                if (catEncyclopediaList.TryGetValue(collectedCatId, out var catViewModel))
                {
                    catViewModel.IsCollected = true;
                }
                else
                {
                    Debug.LogWarning($"도감에서 저장된 고양이 ID를 찾을 수 없습니다: {collectedCatId}");
                }
            }
        }

        economyService.GetEconomyViewModel();
        economyService.UpdateSpecialCatEffects();
    }

    private void LoadGold()
    {
        var economyVM = GameManager.Instance.EconomyService_DH.GetEconomyViewModel();
        economyVM.CurrentGold = _saveData.Gold;
    }

    private void LoadFish()
    {
        var economyVM = GameManager.Instance.EconomyService_DH.GetEconomyViewModel();
        economyVM.CurrentFish = _saveData.Fish;
    }

    private void LoadLandInfo()
    {
        GameManager.Instance.MapManager._lvm.RestoreLandLevel(_saveData.LandLevel);
    }

    private async UniTask LoadPlaceBuilding()
    {
        if (_saveData.Buildings == null)
            return;

        foreach (PlacedBuildingSaveData buildingSaveData in _saveData.Buildings)
        {
            await GameManager.Instance.BuildManager.OnLoadBuild(buildingSaveData);
        }
    }

    private void SaveCollectedSpecialCatList()
    {
        _saveData.CollectedCatIdList.Clear();
        foreach (var catVM in GameManager.Instance.EconomyService_DH.CatEncyclopediaList.Values)
        {
            if(catVM.IsCollected == true)
                _saveData.CollectedCatIdList.Add(catVM.CatInfoDataId);
        }
    }

    private void SavePlacedBuildingList()
    {
        _saveData.Buildings.Clear();

        foreach (var buildingData in GameManager.Instance.MapManager._currentBuildingLDic.Values)
        {
            PlacedBuildingSaveData buildingSaveData = new()
            {
                BuildingId = buildingData.BuildingID,
                ModelAddress = buildingData.ModelAddress,
                RootX = buildingData.RootX
            };
            _saveData.Buildings.Add(buildingSaveData);
        }
    }


}
