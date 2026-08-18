
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
    LoadLandUpGradeData,
    Complete
}

public enum DataType
{
    TestData,
    BuildingData,
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


