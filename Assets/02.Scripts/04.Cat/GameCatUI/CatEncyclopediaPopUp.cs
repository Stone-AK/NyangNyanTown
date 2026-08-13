using System.Collections.Generic;
using UnityEngine;

public class CatEncyclopediaPopUp : MonoBehaviour
{
    private List<CatEncyclopediaViewModel> _catEncyclopediaList = new();

    private void InitCatEncyclopedList()
    {
        if (GameManager.Instance.DataManager.TryGetDataTable<CatInfoData>(out var dataTable))
        {
            foreach (var data in dataTable)
            {
                CatEncyclopediaViewModel newCatData = new();
                newCatData.CatInfoDataId = data.Key;
                _catEncyclopediaList.Add(newCatData);
            }
        }
    }

}
