using System.Collections.Generic;
using UnityEngine;

public class TestBuildingDatabase : MonoBehaviour
{
   [SerializeField] private List<Mesh> _meshList;

    public List<BuildingData> BuildingDatas { get; private set; }
    public static TestBuildingDatabase Instance { get; private set; }
   
    private void Awake()
    { 
        Instance = this;
        BuildingDatas = new List<BuildingData>()
        {
            new BuildingData()
            {
                Id = 0,
                Name = "HOUSE",
                Cost = 100,
                Width = 1f,
                Mesh = _meshList[0]
            },

            new BuildingData()
            {
                Id = 1,
                Name = "FARM",
                Cost = 300,
                Width = 1.5f,
                Mesh = _meshList[1]
            },

            new BuildingData()
            {
                Id = 2,
                Name = "SHOP",
                Cost = 500,
                Width = 2f,
                Mesh = _meshList[2]
            },
            new BuildingData()
            {
                Id = 3,
                Name = "CITYHALL",
                Cost = 1200,
                Width = 2f,
                Mesh = _meshList[3]
            }
        };
    }
}