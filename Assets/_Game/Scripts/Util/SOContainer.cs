using System;
using System.Linq;
using UnityEngine;

public class SOContainer : MonoBehaviour
{
    [SerializeField] ScriptableObject[] Container;

    public static SOContainer Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public T GetSO<T>()
    {
        var so = Container.Where(s => typeof(T).IsAssignableFrom(s.GetType())).FirstOrDefault();
        return (T)Convert.ChangeType(so, typeof(T));
    }
}