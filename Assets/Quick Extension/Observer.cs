using System.Collections.Generic;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

public static class Observer
{
    static readonly Dictionary<string, HashSet<UnityAction<object>>> observer = new();

    static HashSet<UnityAction<object>> GetActionList(string key)
    {
        if (observer.ContainsKey(key))
        {
            return observer[key];
        }
        else
        {
            observer.Add(key, new HashSet<UnityAction<object>>());
            return observer[key];
        }
    }

    public static void AddEvent(string key, UnityAction<object> action)
    {
        GetActionList(key).Add(action);
    }

    public static void RemoveEvent(string key)
    {
        if (observer.ContainsKey(key)) observer.Remove(key);
        else Debug.LogWarning("Không tồn tại sự kiện " + key);
    }

    public static void RemoveActionEvent(string key, UnityAction<object> action)
    {
        if (GetActionList(key).Contains(action)) GetActionList(key).Remove(action);
    }

    public static void Notify(string key, object data = null)
    {
        HashSet<UnityAction<object>> actions = GetActionList(key);
        foreach (UnityAction<object> action in actions) action?.Invoke(data);
    }
}
