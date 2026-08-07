using UnityEngine;

public class CurrencyService
{
  private CurrencyViewModel CurrencyViewModel;

    public CurrencyViewModel GetCurrencyViewModel()
    {
        if(CurrencyViewModel == null)
        {
            CreateCurrencyViewModel();
        }

        return CurrencyViewModel;
    }

    public CurrencyViewModel CreateCurrencyViewModel()
    {
        var currencyViewModel = new CurrencyViewModel();
        currencyViewModel.Gold = 0;
        currencyViewModel.Fish = 0;
        currencyViewModel.CatCurrentCount = 0;
        currencyViewModel.CatMaxCount = 0;

        return currencyViewModel;
    }

    public void AddGoldCurrency(int Gold)
    {
        if (CurrencyViewModel != null)
        {
            CurrencyViewModel.Gold += Gold;
        }
    }

    public void RemoveGoldCurrency(int Gold)
    {
        if (CurrencyViewModel != null)
        {
            CurrencyViewModel.Gold -= Gold;
        }
    }

    public void AddFishCurrency(int Fish)
    {
        if (CurrencyViewModel != null)
        {
            CurrencyViewModel.Fish += Fish;
        }
    }

    public void RemoveFishCurrency(int Fish)
    {
        if (CurrencyViewModel != null)
        {
            CurrencyViewModel.Fish -= Fish;
        }
    }

    public void AddCatCurrentCountCurrency(int CatCurrentCount)
    {
        if( CurrencyViewModel != null)
        {
            CurrencyViewModel.CatCurrentCount += CatCurrentCount;
        }
    }

    public void RemoveCatCurrentCountCurrency(int CatCurrentCount)
    {
        if (CurrencyViewModel != null)
        {
            CurrencyViewModel.CatCurrentCount -= CatCurrentCount;
        }
    }

    public void AddCatMaxCountCurrency(int CatMaxCount)
    {
        if (CurrencyViewModel != null)
        {
            CurrencyViewModel.CatMaxCount += CatMaxCount;
        }
    }

    public void RemoveCatMaxCountCurrency(int CatMaxCount)
    {
        if (CurrencyViewModel != null)
        {
            CurrencyViewModel.CatMaxCount -= CatMaxCount;
        }
    }
}
