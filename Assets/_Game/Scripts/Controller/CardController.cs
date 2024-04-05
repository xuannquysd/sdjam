using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CardController : MonoBehaviour
{
    [SerializeField] byte id;
    [SerializeField] TMP_Text xTxt;
    [SerializeField] GameObject border, greenBorder;

    Animator animator;
    Button button;

    public byte Id { get => id; set => id = value; }

    private void Start()
    {
        if (!gameObject.activeSelf) return;

        animator = GetComponent<Animator>();
        animator.enabled = false;

        xTxt.text = "";
    }

    public void InitCallBack(UnityAction callback)
    {
        if (!gameObject.activeSelf) return;

        button = GetComponent<Button>();
        button.onClick.AddListener(callback);
    }

    public void SetText(string text)
    {
        if (!gameObject.activeSelf) return;

        xTxt.text = text;
    }

    public void OnSelect(bool active)
    {
        if (!gameObject.activeSelf) return;

        border.SetActive(active);
    }

    public void Win()
    {
        if (!gameObject.activeSelf) return;

        greenBorder.SetActive(true);
        animator.enabled = true;
        animator.Rebind();
    }

    public void Clear()
    {
        if (!gameObject.activeSelf) return;

        border.SetActive(false);
        animator.enabled = false;
        greenBorder.SetActive(false);
    }
}
