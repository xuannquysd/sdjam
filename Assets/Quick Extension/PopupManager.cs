using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PopupManager : SingletonDontDestroy<PopupManager>
{
    readonly Stack<GameObject> popups = new();

    public GameObject ShowPopup(string path, Transform parent = null)
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        GameObject popup = Instantiate(prefab, parent);
        popups.Push(popup);
        return popup;
    }

    public void HidePopup()
    {
        GameObject popup = popups.Pop();
        if(popup == null)
        {
            HidePopup();
            return;
        }

        Destroy(popup);
    }
}
