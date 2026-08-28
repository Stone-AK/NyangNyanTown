using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GachaManager : BaseManager<GachaManager>
{
    Dictionary<string, float> rewardProbabilities = new Dictionary<string, float>
{
    { "Gold", 80.0f },
    { "Cat", 20.0f },
};

    private List<RewardData> rewardList = new();

    private int _goldValue = 100;
    private int _ifCatGoldValue = 20;


    public override UniTask InitializeAsync()
    {
        return UniTask.CompletedTask;
    }

    public async UniTask TryGachaByCount(int count)
    {

        rewardList.Clear();
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

        var view = await GameManager.Instance.UIManager.OpenRewardPopupUIAsync();
        view.Bind(rewardList);
    }



    private void WinGold()
    {
        var rewredData = SetReward(nameof(RewardType.Gold), RewardType.Gold, _goldValue);
        rewardList.Add(rewredData);

        GameManager.Instance.EconomyService_DH.AddCurrentGold(_goldValue);
    }

    private void WinCat()
    {
        string catId = GameManager.Instance.CatManager.SelectRandomCatIdByWeight();

       


        if (GameManager.Instance.EconomyService_DH.CheckClickCatIsNew(catId))
        {
            Debug.Log("새로운 고양이 습득");
            var rewredData = SetReward(catId, RewardType.Cat, 1);
            rewardList.Add(rewredData);
        }
        else
        {
            Debug.Log("이미 습득한 고양이");
            var rewredData = SetReward(nameof(RewardType.Gold), RewardType.Gold, _ifCatGoldValue);
            rewardList.Add(rewredData);
            GameManager.Instance.EconomyService_DH.AddCurrentGold(_ifCatGoldValue);
        }
    }

    public void SetCatGachaByWeight()
    {
        string catId = GameManager.Instance.CatManager.SelectRandomCatIdByWeight();


       
    }


    private RewardData SetReward(string id, RewardType rewardType, int amout)
    {
        var rewredData = new RewardData();
        rewredData.Id = id;
        rewredData.Type = rewardType;
        rewredData.Amount = amout;

        return rewredData;

    }
}
