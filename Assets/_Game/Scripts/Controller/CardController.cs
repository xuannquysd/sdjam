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
        animator = GetComponent<Animator>();
        animator.enabled = false;

        xTxt.text = "";
    }

    public void InitCallBack(UnityAction callback)
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(callback);
    }

    public void SetText(string text)
    {
        xTxt.text = text;
    }

    public void OnSelect(bool active)
    {
        border.SetActive(active);
    }

    public void Win()
    {
        greenBorder.SetActive(true);
        animator.enabled = true;
        animator.Rebind();
    }

    public void Clear()
    {
        border.SetActive(false);
        animator.enabled = false;
        greenBorder.SetActive(false);
    }
}
