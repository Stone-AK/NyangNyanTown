
public enum LoadingState
{
    Loading,
    Ready
}

public enum LoadingStep
{
    None,
    Initialize,
    LoadTestData,
    LoadBuildingData,
    LoadCatInfoData,
    LoadCatBodySkin,
    LoadCatEyeSkin,
    LoadCatMouthSkin,
    LoadLandUpGradeData,
    Complete
}

public enum DataType
{
    TestData,
    BuildingData,
    CatInfoData,
    CatBodySkinData,
    CatEyeSkinData,
    CatMouthSkinData
    CatInfoData,
    LandUpGradeData
}

public enum PrefabType
{
    UILayer,
    AudioView,
    ObjectPoolRoot,
}
public enum BuildingType
{
    Normal,
    TownHall,
    Spawner
}


