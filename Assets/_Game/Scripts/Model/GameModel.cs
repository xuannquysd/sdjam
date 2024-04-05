using System;
using System.Collections.Generic;

[Serializable]
public class BetData
{
    public int Index;
    public byte IdCard;
    public long Money;
}

[Serializable]
public class ResponseData
{
    public byte[] cards;
    public byte[] winedCards;
    public SpecialReward specialReward;
    public long totalReward;
    public string status;
}

[Serializable]
public class SpecialReward
{
    public int tpe;
    public string name;
}

[Serializable]
public class BetHistory
{
    public byte[] Cards;
}