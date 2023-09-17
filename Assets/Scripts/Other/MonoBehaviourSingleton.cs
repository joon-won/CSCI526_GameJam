using UnityEngine;

/// <summary>
/// Generic singleton for <see cref="MonoBehaviour"/>. 
/// </summary>

public abstract class MonoBehaviourSingleton<T> : MonoBehaviour
    where T : MonoBehaviour {

    private static T instance = null;
    public static T Instance {
        get {
            if (instance == null) FindInstance();
            return instance;
        }
    }

    private static void FindInstance() {
        instance = (T)FindFirstObjectByType(typeof(T));

        if (FindObjectsByType(typeof(T), FindObjectsSortMode.None).Length > 1) {
            Debug.LogError($"[Singleton] More than one instance of '{typeof(T)}'.");
        }
        else if (instance == null) {
            Debug.LogWarning($"[Singleton] No instance of '{typeof(T)}' is found.");

            var singletonObject = new GameObject();
            instance = singletonObject.AddComponent<T>();
            singletonObject.name = $"(Temp) {typeof(T)}";

            Debug.LogWarning($"[Singleton] Created a temp instance of '{typeof(T)}'. ");
        }
    }

    protected virtual void Awake() {
        if (instance == null) {
            FindInstance();
        }
        else if (instance != this as T) {
            Destroy(gameObject);
            enabled = false;
            Debug.LogWarning($"Singleton<{typeof(T).Name}>: Destroyed duplicate GameObject ({name}). ");
            return;
        }
    }
}