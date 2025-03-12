using UnityEngine;

public abstract class SingletonMO<T> : MonoBehaviour where T : SingletonMO<T>
{
    private static T instance;
    private static object lockObject = new object();
    private static bool applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed. Returning null.");
                return null;
            }

            lock (lockObject)
            {
                if (instance == null)
                {
                    instance = FindFirstObjectByType<T>();

                    if (instance == null)
                    {
                        GameObject singletonObject = new GameObject();
                        instance = singletonObject.AddComponent<T>();
                        singletonObject.name = $"{typeof(T).Name} (Singleton)";

                        DontDestroyOnLoad(singletonObject);
                    }
                }
                return instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            if (ShouldPersistAcrossScenes())
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else if (instance != this)
        {
            Debug.LogWarning($"[Singleton] Multiple instances of '{typeof(T)}' found. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    protected virtual bool ShouldPersistAcrossScenes() => false;

    protected virtual void OnApplicationQuit()
    {
        applicationIsQuitting = true;
        instance = null;
        Destroy(gameObject);
    }
}