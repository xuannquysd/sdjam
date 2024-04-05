using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinPopup : PopupUI
{
    [SerializeField] TMP_Text winMoneyTxt;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] Button claimButton;

    public void InitUI(long winMoney)
    {
        StartCoroutine(ShowMoneyEffect(winMoney));

        claimButton.onClick.AddListener(() =>
        {
            ShowCoinEffect();
            GameManager.Instance.GameState = GameState.WAIT;

            SessionPref.AddMoney(winMoney);
            PopupManager.Instance.HidePopup();
        });
    }

    IEnumerator ShowMoneyEffect(long winMoney)
    {
        long displayMoney = 0;
        long offset = winMoney / 50;

        winMoneyTxt.text = "<size=120%><sprite index=0></size>" + FormatText.GetFormatText(displayMoney);

        for (int i = 0; i < 50; i++)
        {
            yield return null;
            displayMoney += offset;
            winMoneyTxt.text = "<size=120%><sprite index=0></size>" + FormatText.GetFormatText(displayMoney);
        }

        winMoneyTxt.text = "<size=120%><sprite index=0></size>" + FormatText.GetFormatText(winMoney);
    }

    void ShowCoinEffect()
    {
        Transform pointTarget = GameManager.Instance.CoinUI;
        Transform parent = GameManager.Instance.EffectCanvas;

        Vector3 startPoint = Camera.main.WorldToScreenPoint(Vector3.zero);

        for(int i = 0; i < 10; i++)
        {
            GameObject coin = Instantiate(coinPrefab, startPoint, Quaternion.identity, parent);

            Vector3 offset = new(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0f);
            Vector3 pointOffset = startPoint + offset;

            coin.transform.DOMove(pointOffset, Random.Range(0.25f, 0.5f)).SetEase(Ease.OutSine).OnComplete(() =>
            {
                coin.transform.DOMove(pointTarget.position, Random.Range(0.25f, 0.75f)).SetEase(Ease.InSine).OnComplete(() =>
                {
                    Destroy(coin);
                });
            });
        }

        SoundManager.Instance.SoundCoin();
    }
}