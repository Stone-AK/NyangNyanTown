
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
    LoadAudioData,
    Complete
}

public enum DataType
{
    TestData,
    BuildingData,
    CatInfoData,
    CatBodySkinData,
    CatEyeSkinData,
    CatMouthSkinData,
    LandUpGradeData,
    AudioData
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
    Spawner,
    LandMark
}


