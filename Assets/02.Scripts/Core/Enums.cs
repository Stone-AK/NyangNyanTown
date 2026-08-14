
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
    Complete
}

public enum DataType
{
    TestData,
    BuildingData,
    CatInfoData
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


