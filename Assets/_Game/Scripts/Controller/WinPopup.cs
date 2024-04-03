using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class WinPopup : PopupUI
{
    [SerializeField] TMP_Text winMoneyTxt;
    [SerializeField] GameObject coinPrefab;

    public void InitUI(long winMoney)
    {
        StartCoroutine(ShowMoneyEffect(winMoney));
    }

    IEnumerator ShowMoneyEffect(long winMoney)
    {
        long displayMoney = 0;
        long offset = winMoney / 200;

        winMoneyTxt.text = "+" + displayMoney.ToString();

        for (int i = 0; i < 200; i++)
        {
            yield return null;
            displayMoney += offset;
            winMoneyTxt.text = "+" + displayMoney.ToString();
        }

        yield return new WaitForSeconds(1f);

        ShowCoinEffect();
        PopupManager.Instance.HidePopup();
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