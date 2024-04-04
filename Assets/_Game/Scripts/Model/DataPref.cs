using System;

[Serializable]
public class CurrentData 
{
    public long CurrentMoney;
    public BetData[] BetDatas;

    public CurrentData()
    {
        CurrentMoney = 1000000000;
    }
}
