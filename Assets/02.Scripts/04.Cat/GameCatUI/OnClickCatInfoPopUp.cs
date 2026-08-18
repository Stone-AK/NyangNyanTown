using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnClickCatInfoPopUp : BaseUI
{
    [SerializeField] private TextMeshProUGUI _catNameText;
    [SerializeField] private TextMeshProUGUI _catCommitText;

    public void SettingPopUp(CatView chooseCat)
    {
        CatView clickededCat = chooseCat;

        string catName = null;
        string catCommit = null;

        if(GameManager.Instance.DataManager.TryGetDataTable<CatInfoData>(out var catDataTable))
        {
            catName = catDataTable[clickededCat.CatViewModelProp.CatId].Name;
            catCommit = catDataTable[clickededCat.CatViewModelProp.CatId].Description;
        }

        if(catName != null || catCommit != null)
        {
            _catNameText.text = catName;
            _catCommitText.text = catCommit;
        }
    }

    public void ClosePopUp()
    {
        GameManager.Instance.UIManager.Close(UIType.OnClickCatInfoPopUp);
    }
}
