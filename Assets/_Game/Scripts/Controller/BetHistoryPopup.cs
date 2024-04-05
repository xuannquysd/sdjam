using UnityEngine;
using UnityEngine.UI;

public class BetHistoryPopup : PopupUI
{
    [SerializeField] HistoryChild historyChildPrefab;
    [SerializeField] Transform listContainer;
    [SerializeField] Button closeBtn;

    private void Awake()
    {
        closeBtn.onClick.AddListener(() => Destroy(gameObject));
    }

    private void Start()
    {
        Init();
    }

    void Init()
    {
        BetHistory[] betHistory = SessionPref.GetHistory();

        int index = 1;

        for(int i = betHistory.Length - 1; i>= 0; i--)
        {
            int tempIndex = index;
            HistoryChild historyChild = Instantiate(historyChildPrefab, listContainer);
            historyChild.Init(tempIndex, betHistory[i].Cards);
            index++;

            if (index > 10) break;
        }
    }
}