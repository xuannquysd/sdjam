using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Game Datas/CardData")]
public class CardData : ScriptableObject
{
    public Card[] Cards;

    [Serializable]
    public struct Card
    {
        public byte idCard;
        public Sprite icon;
        public CardType Type;
    }
}