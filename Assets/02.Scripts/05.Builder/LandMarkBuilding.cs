using UnityEngine;

public class LandMarkBuilding : MonoBehaviour
{
    [SerializeField] GameObject Panel;

    public void OnBuild() 
    {
        Panel.SetActive(false);
    }
}
