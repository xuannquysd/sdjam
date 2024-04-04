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
        float speed = 0f;
        float targetSpeed = 1500f;

        while (true)
        {
            yield return null;

            if (speed < targetSpeed) speed += 1000f * Time.deltaTime;
            fakeSlide.anchoredPosition += speed * Time.deltaTime * Vector2.down;
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
                fakeSlide.DOAnchorPosY(-140f * 14f, 0.5f).SetEase(Ease.OutBack, -100f);
                break;
            }
        }
    }
}
