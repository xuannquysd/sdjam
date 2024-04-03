using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlotSpin : MonoBehaviour
{
    [SerializeField] RectTransform fakeSlide;
    [SerializeField] Image resultIcon;

    Coroutine spinCoroutine;

    public void FakeSpin()
    {
        spinCoroutine = StartCoroutine(Spin());
    }

    IEnumerator Spin()
    {
        fakeSlide.anchoredPosition = Vector2.zero;

        while (true)
        {
            yield return null;

            fakeSlide.anchoredPosition += 1500f * Time.deltaTime * Vector2.down;
            if (fakeSlide.anchoredPosition.y < -140f * 12f)
            {
                float offset = 140f * 12f + fakeSlide.anchoredPosition.y;
                fakeSlide.anchoredPosition = new(0f, -offset);
            }
        }
    }

    public IEnumerator ShowReward(Sprite icon)
    {
        StopCoroutine(spinCoroutine);
        resultIcon.sprite = icon;

        fakeSlide.anchoredPosition = Vector2.zero;

        while (true)
        {
            yield return null;
            
            if (fakeSlide.anchoredPosition.y > -140f * 14f)
            {
                fakeSlide.anchoredPosition += 1500f * Time.deltaTime * Vector2.down;
            }
            else
            {
                fakeSlide.anchoredPosition = new(0f, -140f * 14f);
                break;
            }
        }
    }
}
