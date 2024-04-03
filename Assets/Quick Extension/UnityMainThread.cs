using System.Collections.Generic;
using UnityEngine.Events;

public class UnityMainThread : SingletonDontDestroy<UnityMainThread>
{
    readonly Queue<UnityAction> actions = new();

    public void AddAction(UnityAction action)
    {
        actions.Enqueue(action);
    }

    private void Update()
    {
        while(actions.Count > 0)
        {
            UnityAction action = actions.Dequeue();
            action?.Invoke();
        }
    }
}
