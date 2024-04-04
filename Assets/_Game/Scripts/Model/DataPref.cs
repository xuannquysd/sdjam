using System;

[Serializable]
public class CurrentData 
{
    public long CurrentMoney;
    public bool isRemoveAds;
    public BetData[] BetDatas;

    public CurrentData()
    {
        CurrentMoney = 1000000000;
        isRemoveAds = false;
    }
}
