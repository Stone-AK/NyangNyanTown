using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class GachaManager : BaseManager<GachaManager>
{
    Dictionary<string, float> rewardProbabilities = new Dictionary<string, float>
{
    { "Gold", 80.0f },
    { "Cat", 20.0f },
};

    public override UniTask InitializeAsync()
    {
        return UniTask.CompletedTask;
    }

    public void TryGachaByCount(int count)
    {
        for (int i = 0; i < count; i++)
        {

            var gacha = GameUtil.PickByProbability(rewardProbabilities);

            switch (gacha)
            {
                case "Gold":
                    WinGold();
                    break;

                case "Cat":
                    WinCat();
                    break;

                default:
                    Debug.LogError($"처리되지 않은 가챠 결과: {gacha}");
                    break;
            }
        }

    }



    private void WinGold()
    {
        GameManager.Instance.EconomyService_DH.AddCurrentGold(100);
    }

    private void WinCat()
    {
        //string catId = GameManager.Instance.CatManager.SelectRandomCatIdByWeight();

        //if (GameManager.Instance.DataManager.TryGetData(catId, out CatInfoData catInfoData)) { }

        SetCatGachaByWeight();
    }

    public void SetCatGachaByWeight()
    {
        string catId = GameManager.Instance.CatManager.SelectRandomCatIdByWeight();


        if (GameManager.Instance.EconomyService_DH.CheckClickCatIsNew(catId))
        {
            Debug.Log("새로운 고양이 습득");
        }
        else
        {
            Debug.Log("이미 습득한 고양이");
        }
    }
}
