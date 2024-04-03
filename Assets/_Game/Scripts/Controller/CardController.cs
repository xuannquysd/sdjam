using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CardController : MonoBehaviour
{
    [SerializeField] byte id;
    [SerializeField] TMP_Text xTxt;

    Button button;

    public byte Id { get => id; set => id = value; }

    private void Start()
    {
        xTxt = transform.GetChild(0).GetComponent<TMP_Text>();
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
}
