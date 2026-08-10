using UnityEngine;

public class BuildingPanelViewModel
{
    [SerializeField] private GameObject _dataBase;
    //얘가 해야할거 데이터 리스트 받아와서 초기화 시켜주고
    public void Initailize() 
    {
        TestBuildingDatabase testBuildingDatabase = _dataBase.GetComponent<TestBuildingDatabase>();
        foreach (var data in testBuildingDatabase.BuildingDatas)
        {
           
        }

    }
}
