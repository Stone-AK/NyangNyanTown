using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class CatManager : MonoBehaviour
{
    public static CatManager Instance { get; set; }

    [SerializeField] private CatView CatPrefab;

    // TODO(안우재/08.06) : 추후 고양이 인구수가 할당되어야 함.
    // 게임 데이터의 고양이 인구수를 뷰모델의 데이터로 하여 변경 시 Poolling되는
    // 오브젝트 수 관리 및 소환 제한을 할 수 있도록 해야함.
    public int PoolSize { get; private set; } = 1;
    public List<CatView> CatPool = new();
    private int ActiveCatCount;
    public List<CatSpawner> CatSpanweList = new();

    private void Awake()
    {
        Instance = this;
        InitPool();
    }

    private void InitPool()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            CatView cat = Instantiate(CatPrefab, transform);

            cat.gameObject.SetActive(false);
            CatPool.Add(cat);
        }
    }

    // 추후 게임매니저 데이터(고냥이 인구수)와의 연동 시 필요
    private void AddCatToCatPool()
    {
        for (int i = CatPool.Count; i < PoolSize; i++)
        {
            CatView cat = Instantiate(CatPrefab, transform);

            cat.gameObject.SetActive(false);
            CatPool.Add(cat);
        }
    }

    public CatView GetCatFromPool()
    {
        if(ActiveCatCount >= PoolSize)
        {
            Debug.LogWarning("고양이 인구수가 최대치입니다.");
            return null;
        }

        foreach (var cat in CatPool)
        {
            if (!cat.gameObject.activeSelf)
            {
                cat.InitCatView(InitRandomCatStat());

                ActiveCatCount++;
                return cat;
            }
        }

        Debug.LogWarning("고냥이가 부족합니다.");
        return null;
    }

    public void DeActiveCat(CatView cat)
    {
        if (cat != null)
        {
            cat.gameObject.SetActive(false);
            ActiveCatCount--;
        }
    }

    private CatViewModel InitRandomCatStat()
    {
        CatViewModel catViewModel = new CatViewModel();
        catViewModel.InitRandomCatStat();
        return catViewModel;
    }

    public bool IsCatSpawnAvailable()
    {
        return ActiveCatCount < PoolSize;
    }
}
