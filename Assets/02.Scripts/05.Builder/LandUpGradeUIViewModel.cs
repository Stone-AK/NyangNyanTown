using UnityEngine;

public class LandUpGradeUIViewModel
{
    public int CurrentGold { get; set; }
    public int NeedGold { get; set; }
    public int CurrentCat { get; set; }
    public int NeedCat { get; set; }
    public string NeedBuilding { get; set; }
    public string NeedSpecialCat { get; set; }

    private LandUpGradeData _landUpGradeData;

    public LandUpGradeUIViewModel(LandUpGradeData landUpGradeData) 
    {
        _landUpGradeData = landUpGradeData;
    }
}
