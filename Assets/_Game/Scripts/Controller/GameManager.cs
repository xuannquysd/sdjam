using System;
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
    [SerializeField] Button bet1kBtn, bet10kBtn, bet100kBtn, cancelBet, spinBtn;
    [SerializeField]
    SlotSpin[] slotSpin;
    [SerializeField] TMP_Text moneyText;
    [SerializeField] List<BetData> bets;
    [SerializeField] Transform coinUI;
    [SerializeField] Transform effectCanvas;

    Coroutine waitSpin;

    public GameState GameState;

    private long currentPointBet;

    public static GameManager Instance;

    public long CurrentPointBet
    {
        get => currentPointBet; set
        {
            currentPointBet = value;
        }
    }

    public Transform CoinUI { get => coinUI; set => coinUI = value; }
    public Transform EffectCanvas { get => effectCanvas; set => effectCanvas = value; }

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

        CurrentPointBet = 1000;
        bets = new();

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
        bet1kBtn.onClick.AddListener(() => OnClickChangeValueBet(1000));
        bet10kBtn.onClick.AddListener(() => OnClickChangeValueBet(10000));
        bet100kBtn.onClick.AddListener(() => OnClickChangeValueBet(100000));
        cancelBet.onClick.AddListener(OnClickCancelBet);
        spinBtn.onClick.AddListener(OnClickSpin);
    }

    void OnClickChangeValueBet(long value)
    {
        if (GameState != GameState.WAIT) return;

        CurrentPointBet = value;
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

        SoundManager.Instance.SoundBet();

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
        if(bets.Count == 0) return;

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

            SoundManager.Instance.SoundSpin();
            for(int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(1.5f);
                Sprite icon = cardData.Cards.Where(c => c.idCard == responseData.cards[i]).First().icon;
                waitSpin = StartCoroutine(slotSpin[i].ShowReward(icon));
            }

            yield return waitSpin;

            SoundManager.Instance.StopSount();
            long winMoney = responseData.totalReward;
            SessionPref.AddMoney(winMoney);

            if(winMoney > 0) ShowEffectWin(winMoney);
            //Clear();
        }

        GameState = GameState.WAIT;
    }

    private void ShowEffectWin(long totalReward)
    {
        WinPopup winPopup = PopupManager.Instance.ShowPopup<WinPopup>();
        winPopup.InitUI(totalReward);
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
