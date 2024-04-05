using System;

[Serializable]
public class CurrentData 
{
    public long CurrentMoney;
    public bool isRemoveAds;
    public BetData[] BetDatas;
    public BetHistory[] BetHistory;

    public CurrentData()
    {
        CurrentMoney = 1000000000;
        isRemoveAds = false;
        BetHistory = new BetHistory[0];
    }
}
