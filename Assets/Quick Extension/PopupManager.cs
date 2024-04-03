using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [SerializeField] PopupUI[] popupPrefab;
    readonly Stack<GameObject> popups = new();

    public static PopupManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public T ShowPopup<T>(Transform parent = null) where T : PopupUI
    {
        PopupUI prefab = popupPrefab.Where(popup => typeof(T).IsAssignableFrom(popup.GetType())).FirstOrDefault();

        if (prefab == null)
        {
            throw new NullReferenceException("Ko tim thay popup");
        }
        else
        {
            PopupUI popup = Instantiate(prefab, parent);
            popups.Push(popup.gameObject);
            return popup.GetComponent<T>();
        }
    }

    public void HidePopup()
    {
        GameObject popup = popups.Pop();
        if (popup == null)
        {
            HidePopup();
            return;
        }

        Destroy(popup);
    }
}