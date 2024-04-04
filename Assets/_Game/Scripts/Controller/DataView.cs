using UnityEngine;

public class DataView : MonoBehaviour
{
    [SerializeField] CurrentData CurrentData;

    private void Start()
    {
        CurrentData = SessionPref.GetCurrentData();
    }
}