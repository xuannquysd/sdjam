using UnityEngine;
using Unity.Collections;
using System.Collections;

public class NotiPopup : PopupUI
{
    private void Start()
    {
        StartCoroutine(Clear());
    }

    IEnumerator Clear()
    {
        yield return new WaitForSeconds(2f);
        PopupManager.Instance.HidePopup();

        GameManager.Instance.IsShowNoti = false;
    }
}