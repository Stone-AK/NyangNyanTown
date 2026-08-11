using UnityEngine;

public class UILayer : MonoBehaviour
{
    [SerializeField] private RectTransform _main;
    [SerializeField] private RectTransform _popup;
    [SerializeField] private RectTransform _veryFront;

    public RectTransform Main
    { 
        get { return _main; } 
    }

    public RectTransform Popup 
    {
        get { return _popup; } 
    }

    public RectTransform VeryFront
    { 
        get { return _veryFront; }
    }

   

    private void Awake()
    {
        GameUtil.ValidateReference(_main, nameof(UILayer), nameof(_main));
        GameUtil.ValidateReference(_popup, nameof(UILayer), nameof(_popup));
        GameUtil.ValidateReference(_veryFront, nameof(UILayer), nameof(_veryFront));
       
    }
}