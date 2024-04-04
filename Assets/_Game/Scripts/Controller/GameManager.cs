using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static CardData;

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

    [SerializeField]
    Image[] betBg;
    [SerializeField] Sprite selectedBet, unselectedBet;
    [SerializeField] CardController khanUI, phudocUI;
    [SerializeField] Button prtScrBtn, tutorialBtn, rewardBtn;

    bool isShowNoti = false;
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
    public bool IsShowNoti { get => isShowNoti; set => isShowNoti = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SessionPref.ReadData();
            DontDestroyOnLoad(gameObject);

            InitActionCard();
            InitBetButton();

            prtScrBtn.onClick.AddListener(OnClickPrtSrc);
            tutorialBtn.onClick.AddListener(OnClickTutorial);
            rewardBtn.onClick.AddListener(OnClickReward);

            InitObserver();
        }
        else Destroy(gameObject);
    }

    private void OnClickReward()
    {
        if (GameState != GameState.WAIT) return;
        PopupManager.Instance.ShowPopup<AdsPopup>();
    }

    private void Start()
    {
        GameState = GameState.WAIT;

        CurrentPointBet = 0;
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
        bet1kBtn.onClick.AddListener(() => OnClickChangeValueBet(1000, 0));
        bet10kBtn.onClick.AddListener(() => OnClickChangeValueBet(10000, 1));
        bet100kBtn.onClick.AddListener(() => OnClickChangeValueBet(100000, 2));
        cancelBet.onClick.AddListener(OnClickCancelBet);
        spinBtn.onClick.AddListener(OnClickSpin);
    }

    void OnClickChangeValueBet(long value, int index)
    {
        if (GameState != GameState.WAIT) return;

        CurrentPointBet = value;

        for(int i = 0; i < betBg.Length; i++)
        {
            if (i == index) betBg[i].sprite = selectedBet;
            else betBg[i].sprite = unselectedBet;
        }
    }

    void OnClickCancelBet()
    {
        if (GameState != GameState.WAIT) return;

        Clear();
    }

    void Clear()
    {
        bets.Clear();
        foreach (var card in cardBtn)
        {
            card.Clear();
            card.SetText("");
        }
    }

    void OnClickCard(int indexCard)
    {
        if (GameState != GameState.WAIT) return;

        if(CurrentPointBet == 0)
        {
            //Thông báo
            if (isShowNoti) return;
            PopupManager.Instance.ShowPopup<NotiPopup>();
            IsShowNoti = true;

            return;
        }

        if (IsNotEnoughMoney()) return;

        SoundManager.Instance.SoundBet();

        int indexContainCard = IndexContainCard(indexCard);

        cardBtn[indexCard].OnSelect(true);

        if (indexContainCard > -1)
        {
            bets[indexContainCard].Money += CurrentPointBet;
            cardBtn[indexCard].SetText(FormatText.GetFormatText(bets[indexContainCard].Money));
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
            cardBtn[indexCard].SetText(FormatText.GetFormatText(CurrentPointBet));
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

        SaveBet();

        foreach (var card in cardBtn)
        {
            card.Clear();
            if (IsSelected(card.Id)) card.OnSelect(true);
        }

        GameState = GameState.ROLL;

        SessionPref.AddMoney(-TotalCurrentMoneyBet());

        foreach(var slot in slotSpin)
        {
            slot.FakeSpin();
        }

        string data = ConvertToJson();
        StartCoroutine(GetResult(data));
    }

    void SaveBet()
    {
        foreach (var bet in bets)
        {
            SessionPref.AddBetData(bet);
        }
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

            int indexSlot = 0;

            yield return new WaitForSeconds(1.5f);
            for (int i = 0; i < responseData.winedCards.Length; i++)
            {
                Card card = cardData.Cards.Where(c => c.idCard == responseData.winedCards[i]).FirstOrDefault();

                if (card != null)
                {
                    waitSpin = StartCoroutine(slotSpin[indexSlot].ShowReward(card.icon));
                    indexSlot++;
                    yield return new WaitForSeconds(1.5f);
                }
            }

            yield return waitSpin;

            SoundManager.Instance.StopSount();
            long winMoney = responseData.totalReward;
            
            foreach(var card in cardBtn)
            {
                if (IsWin(card.Id, responseData)) card.Win();
            }

            yield return new WaitForSeconds(2f);

            if(winMoney > 0) ShowEffectWin(winMoney);
            else GameState = GameState.WAIT;
        }
    }

    private void ShowEffectWin(long totalReward)
    {
        WinPopup winPopup = PopupManager.Instance.ShowPopup<WinPopup>();
        winPopup.InitUI(totalReward);

        /*foreach (var card in cardBtn)
        {
            card.Clear();
            if (IsSelected(card.Id)) card.OnSelect(true);
        }*/
    }

    void InitObserver()
    {
        Observer.AddEvent(Constance.ON_MONEY_CHANGE, SetMoneyText);
    }

    void SetMoneyText(object data)
    {
        long currentMoney = SessionPref.GetCurrentMoney();
        moneyText.text = FormatText.GetFormatText(currentMoney);
    }

    bool IsWin(byte idCard, ResponseData responseData)
    {
        foreach(var card in responseData.winedCards)
        {
            if(card == idCard) return true;
        }

        return false;
    }
    
    bool IsSelected(byte idCard)
    {
        foreach(var bet in bets)
        {
            if(bet.IdCard == idCard) return true;
        }

        return false;
    }

    bool IsKhan(ResponseData responseData)
    {
        byte[] cardWin = responseData.winedCards;
        for(int i = 1; i < cardWin.Length; i++)
        {
            if (cardWin[i] != cardWin[i - 1]) return false;
        }

        return true;
    }

    bool IsPhuDoc(ResponseData responseData)
    {
        byte[] cardWin = responseData.winedCards;
        for (int i = 1; i < cardWin.Length; i++)
        {
            if (cardWin[i] - cardWin[i - 1] != 3) return false;
        }

        return true;
    }

    void OnClickPrtSrc()
    {
        string path = "";
#if UNITY_EDITOR
        path += "Assets/";
#endif
        path += Guid.NewGuid().ToString() + ".png";
        ScreenCapture.CaptureScreenshot(path);
    }

    void OnClickTutorial()
    {
        if (GameState != GameState.WAIT) return;
        PopupManager.Instance.ShowPopup<TutorialPopup>();
    }
}
