using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] Canvas Canvas_Main;
    [SerializeField] Canvas Canvas_Popup;
    [SerializeField] Canvas Canvas_VeryFront;

    public static UIManager Instance { get; set; }

    private Dictionary<UIType, UIBase> _createdUIDic = new Dictionary<UIType, UIBase>();
    private HashSet<UIType> _openedUIDic = new HashSet<UIType>();
}
