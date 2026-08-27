using TMPro;
using UnityEngine;

public enum SaveLoadPopupType
{
    Save,
    Read,
    Load
}

public class SaveLoadCompletePopup : BaseUI
{
    [SerializeField] private TextMeshProUGUI _saveLoadText;

    private Transform _clickedTransform;

    public void SettingSaveText(bool isSuccess)
    {
        if (isSuccess == true)
            _saveLoadText.text = "Save 완료";
        else if (isSuccess == false)
            _saveLoadText.text = "Save 실패";
    }

    public void SettingReadText(bool isReadDataSuccess)
    {
        if (isReadDataSuccess == false)
            _saveLoadText.text = "저장된 파일이 없습니다.";
    }

    public void SettingLoadText(bool isLoadDataSuccess)
    {
        if (isLoadDataSuccess == true)
            _saveLoadText.text = "Load 완료";
        else if (isLoadDataSuccess == false)
            _saveLoadText.text = "Load 실패";
    }

    public void ClosePopUp()
    {
        GameManager.Instance.UIManager.Close(UIType.SaveLoadCompletePopup);
    }
}
