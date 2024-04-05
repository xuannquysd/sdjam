using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HistoryChild : MonoBehaviour
{
    [SerializeField] Image iconCard;
    [SerializeField] TMP_Text indexTxt;
    [SerializeField] Transform container;
    
    public void Init(int index, byte[] Cards)
    {
        indexTxt.text = index.ToString();

        CardData cardData = SOContainer.Instance.GetSO<CardData>();

        foreach(var card in Cards)
        {
            Sprite icon = cardData.Cards.Where(c => c.idCard == card).FirstOrDefault().icon;

            Image child = Instantiate(iconCard, container);
            child.sprite = icon;
        }
    }
}