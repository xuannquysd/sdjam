using UnityEngine;

public class SingletonDontDestroy<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new();

#if UNITY_EDITOR
                obj.name = typeof(T).FullName;
#endif

                instance = obj.AddComponent<T>();
                DontDestroyOnLoad(obj);
            }

            return instance;
        }
    }
}
