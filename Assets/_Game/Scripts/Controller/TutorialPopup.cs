using UnityEngine;
using UnityEngine.UI;

public class TutorialPopup : PopupUI
{
    [SerializeField] Button closeBtn;

    private void Awake()
    {
        closeBtn.onClick.AddListener(() => PopupManager.Instance.HidePopup());
    }
}