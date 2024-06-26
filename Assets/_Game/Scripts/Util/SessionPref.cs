using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SessionPref
{
    static CurrentData CurrentData;

    public static void ReadData()
    {
        if (PlayerPrefs.HasKey(Constance.CURRENT_DATA_KEY))
        {
            string data = PlayerPrefs.GetString(Constance.CURRENT_DATA_KEY);
            CurrentData = JsonUtility.FromJson<CurrentData>(data);
        }
        else
        {
            CurrentData = new();
            SaveData();
        }
    }

    public static CurrentData GetCurrentData()
    {
        return CurrentData;
    }
    public static void SaveData()
    {
        string data = JsonUtility.ToJson(CurrentData);
        PlayerPrefs.SetString(Constance.CURRENT_DATA_KEY, data);
    }

    public static long GetCurrentMoney()
    {
        return CurrentData.CurrentMoney;
    }

    public static void AddMoney(long amount)
    {
        CurrentData.CurrentMoney += amount;
        SaveData();

        Observer.Notify(Constance.ON_MONEY_CHANGE);
    }

    public static void SetMoney(long amount) 
    { 
        CurrentData.CurrentMoney = amount;
        SaveData();

        Observer.Notify(Constance.ON_MONEY_CHANGE);
    }

    public static void AddBetData(BetData betData)
    {
        CurrentData.BetDatas ??= new BetData[0];
        List<BetData> bets = CurrentData.BetDatas.ToList();
        bets.Add(betData);

        CurrentData.BetDatas = bets.ToArray();
        SaveData();
    }

    public static bool IsRemoveAds()
    {
        return CurrentData.isRemoveAds;
    }

    public static void ActiveRemoveAds()
    {
        CurrentData.isRemoveAds = true;
        SaveData();
    }
}
