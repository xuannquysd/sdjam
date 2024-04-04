using UnityEngine;
using Unity.Collections;
using System.Collections;
using TMPro;

public class NotiPopup : PopupUI
{
    [SerializeField] TMP_Text notiTxt;

    private void Start()
    {
        StartCoroutine(Clear());
    }

    public void SetNoti(string noti)
    {
        notiTxt.text = noti;
    }

    IEnumerator Clear()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);

        GameManager.Instance.IsShowNoti = false;
    }
}