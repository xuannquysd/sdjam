using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] CardController[] cardBtn;
    [SerializeField] Button downBet, upBet, cancelBet, spinBtn;
    [SerializeField] int[] pointBet;
    [SerializeField] TMP_Text pointBetTxt;
    [SerializeField]
    SlotSpin[] slotSpin;
    [SerializeField] TMP_Text moneyText;
    [SerializeField] List<BetData> bets;

    public GameState GameState;

    private int indexPointBet;
    private int currentPointBet;

    public static GameManager Instance;

    public int CurrentPointBet
    {
        get => currentPointBet; set
        {
            currentPointBet = value;
            pointBetTxt.text = value.ToString();
        }
    }

    public int IndexPointBet
    {
        get => indexPointBet; set
        {
            indexPointBet = value;
            CurrentPointBet = pointBet[value];
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SessionPref.ReadData();
            DontDestroyOnLoad(gameObject);

            InitActionCard();
            InitBetButton();

            InitObserver();
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        GameState = GameState.WAIT;

        bets = new();
        IndexPointBet = 0;

        SetMoneyText(null);
    }

    void InitActionCard()
    {
        for (int i = 0; i < cardBtn.Length; i++)
        {
            int tempIndex = i;
            cardBtn[tempIndex].InitCallBack(() => OnClickCard(tempIndex));
        }
    }

    void InitBetButton()
    {
        downBet.onClick.AddListener(() => OnClickChangeValueBet(-1));
        upBet.onClick.AddListener(() => OnClickChangeValueBet(1));
        cancelBet.onClick.AddListener(OnClickCancelBet);
        spinBtn.onClick.AddListener(OnClickSpin);
    }

    void OnClickChangeValueBet(int offset)
    {
        if (GameState != GameState.WAIT) return;

        if (IndexPointBet + offset < 0 || IndexPointBet + offset >= pointBet.Length) return;
        IndexPointBet += offset;
        CurrentPointBet = pointBet[IndexPointBet];
    }

    void OnClickCancelBet()
    {
        if (GameState != GameState.WAIT) return;

        Clear();
    }

    void Clear()
    {
        for(int i = 0; i < bets.Count; i++)
        {
            int index = bets[i].Index;

            cardBtn[index].SetText("");
            bets.RemoveAt(i);
            i--;
        }
    }

    void OnClickCard(int indexCard)
    {
        if (GameState != GameState.WAIT) return;

        if (IsNotEnoughMoney()) return;

        int indexContainCard = IndexContainCard(indexCard);

        if (indexContainCard > -1)
        {
            bets[indexContainCard].Money += CurrentPointBet;
            cardBtn[indexCard].SetText(bets[indexContainCard].Money.ToString());
        }
        else
        {
            BetData bet = new()
            {
                Index = indexCard,
                IdCard = cardBtn[indexCard].Id,
                Money = CurrentPointBet
            };

            bets.Add(bet);
            cardBtn[indexCard].SetText(CurrentPointBet.ToString());
        }
    }

    int IndexContainCard(int idCard)
    {
        bets ??= new();

        for (int i = 0; i < bets.Count; i++) if (bets[i].Index == idCard) return i;

        return -1;
    }

    long TotalCurrentMoneyBet()
    {
        long totalBet = 0;
        foreach (var bet in bets) totalBet += bet.Money;

        return totalBet;
    }

    bool IsNotEnoughMoney()
    {
        long total = TotalCurrentMoneyBet() + CurrentPointBet;
        long currentMoney = SessionPref.GetCurrentMoney();
        return total > currentMoney;
    }

    void OnClickSpin()
    {
        if (GameState != GameState.WAIT) return;
        GameState = GameState.ROLL;

        SessionPref.AddMoney(-TotalCurrentMoneyBet());

        foreach(var slot in slotSpin)
        {
            slot.FakeSpin();
        }

        string data = ConvertToJson();
        StartCoroutine(GetResult(data));
    }

    string ConvertToJson()
    {
        string data = "{ \"datas\": [" ;

        for(int  i = 0; i < bets.Count; i++)
        {
            data += "{ \"id\": " + bets[i].IdCard + ", ";
            data += "\"coin\": " + bets[i].Money;
            data += " }";
            if (i < bets.Count - 1) data += ", ";
        }

        data += "]}";

        return data;
    }

    IEnumerator GetResult(string data)
    {
        using UnityWebRequest www = UnityWebRequest.Post("https://dev.sandinh.com/api/jam/spin", data, "application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            ResponseData responseData = JsonUtility.FromJson<ResponseData>(www.downloadHandler.text);
            CardData cardData = SOContainer.Instance.GetSO<CardData>();

            for(int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(1.5f);
                Sprite icon = cardData.Cards.Where(c => c.idCard == responseData.cards[i]).First().icon;
                StartCoroutine(slotSpin[i].ShowReward(icon));
            }

            /*foreach (var bet in bets) 
            {
                if (IsWin(bet.IdCard, responseData)) SessionPref.AddMoney(bet.Money);
            }*/
            SessionPref.AddMoney(responseData.totalReward);

            Clear();
        }

        GameState = GameState.WAIT;
    }

    void InitObserver()
    {
        Observer.AddEvent(Constance.ON_MONEY_CHANGE, SetMoneyText);
    }

    void SetMoneyText(object data)
    {
        long currentMoney = SessionPref.GetCurrentMoney();
        moneyText.text = currentMoney.ToString();
    }

    bool IsWin(byte idCard, ResponseData responseData)
    {
        foreach(var card in responseData.cards)
        {
            if(card == idCard) return true;
        }

        return false;
    }
}
