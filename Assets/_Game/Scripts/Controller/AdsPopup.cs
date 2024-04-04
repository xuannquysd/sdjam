using TMPro;
using UnityEngine;

public class AdsPopup : PopupUI
{
    [SerializeField] TMP_Text title;

    float timer = 3f;
    private void Update()
    {
        if (timer > 0f)
        {
            int timeShow = Mathf.CeilToInt(timer -= Time.deltaTime);
            title.text = "Đây là quảng cáo, bạn sẽ nhận được 10 xu\n" + timeShow;
        }
        else
        {
            SessionPref.AddMoney(10);
            PopupManager.Instance.HidePopup();
        }
    }
}