using UnityEngine;
using Unity.Collections;
using System.Collections;
using TMPro;
using System;

public class NotiPopup : PopupUI
{
    [SerializeField] TMP_Text notiTxt;

    Action callback;

    private void Start()
    {
        StartCoroutine(Clear());
    }

    public void SetNoti(string noti, Action finnalyCallback = null)
    {
        notiTxt.text = noti;
        callback = finnalyCallback;
    }

    IEnumerator Clear()
    {
        yield return new WaitForSeconds(2f);

        callback?.Invoke();
        Destroy(gameObject);

        GameManager.Instance.IsShowNoti = false;
    }
}