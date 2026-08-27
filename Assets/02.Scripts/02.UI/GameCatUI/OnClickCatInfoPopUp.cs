using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnClickCatInfoPopUp : BaseUI
{
    [SerializeField] private TextMeshProUGUI _catNameText;
    [SerializeField] private TextMeshProUGUI _catCommitText;

    private Transform _clickedTransform;

    private void Update()
    {
        CheckLosingTargetOnUpdate();
    }

    public void SettingPopUp(CatView chooseCat)
    {
        if (chooseCat == null)
            return;

        CatView clickededCat = chooseCat;
        _clickedTransform = clickededCat.gameObject.transform;
        CameraController.Instance.SetFollowingTarget(_clickedTransform);

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
        _clickedTransform = null;

        CameraController.Instance.UnassignedFollowingTarget();

        GameManager.Instance.UIManager.Close(UIType.OnClickCatInfoPopUp);
    }

    private void CheckLosingTargetOnUpdate()
    {
        if (_clickedTransform == null || !_clickedTransform.gameObject.activeInHierarchy)
            ClosePopUp();
    }
}
