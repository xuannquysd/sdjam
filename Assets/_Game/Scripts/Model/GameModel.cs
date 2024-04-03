using System;
using System.Collections.Generic;

[Serializable]
public class BetData
{
    public int Index;
    public byte IdCard;
    public int Money;
}

[Serializable]
public class ResponseData
{
    public byte[] cards;
    public SpecialReward specialReward;
    public long totalReward;
    public string status;

    public override string ToString()
    {
        string txt = "cards: ";
        foreach(var card in cards)
        {
            txt += card.ToString() +", ";
        }
        txt += "totalReward: " + totalReward;
        txt += "status: " + status;

        return txt;
    }
}

[Serializable]
public class SpecialReward
{
    public int tpe;
    public string name;
}