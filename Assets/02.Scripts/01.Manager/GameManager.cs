using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public CurrencyService CurrencyService { get; private set; }

    private void Awake()
    {
        Instance = this;
        InitService();
    }
    
    private void InitService()
    {
        CurrencyService = new CurrencyService();
    }
}
