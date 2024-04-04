using UnityEngine;
using UnityEngine.UI;

public class ShopPopup : PopupUI
{
    [SerializeField] Button removeAdsBtn, coin10Btn, coin100Btn, coint1000Btn, closePopup;
    [SerializeField] GameObject removeAdsItem;

    private void Awake()
    {
        removeAdsBtn.onClick.AddListener(OnClickRemoveAds);
        coin10Btn.onClick.AddListener(() => OnClickBuyCoin(10));
        coin100Btn.onClick.AddListener(() => OnClickBuyCoin(100));
        coint1000Btn.onClick.AddListener(() => OnClickBuyCoin(1000));
        closePopup.onClick.AddListener(() => Destroy(gameObject));
    }

    private void Start()
    {
        InitButton();
    }

    void InitButton()
    {
        if(SessionPref.IsRemoveAds()) removeAdsItem.SetActive(false);
    }

    void OnClickRemoveAds()
    {
        SessionPref.ActiveRemoveAds();
        InitButton();

        NotiPopup notiPopup = PopupManager.Instance.ShowPopup<NotiPopup>();
        notiPopup.SetNoti("Quảng cáo đã được loại bỏ");
    }

    void OnClickBuyCoin(long value)
    {
        SessionPref.AddMoney(value);

        NotiPopup notiPopup = PopupManager.Instance.ShowPopup<NotiPopup>();
        notiPopup.SetNoti("Bạn nhận được " + FormatText.GetFormatText(value) + " xu");
    }
}