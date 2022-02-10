using UnityEngine;

// TODO: Move to Library
// + unique
// + persistent
// + generic

namespace SingletonPattern.Case2.Unity3 {
  public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component {
    private static T _instance;

    public static T Instance => _instance ??= FindObjectOfType<T>() ?? MakeInstance();

    private static T MakeInstance() => new GameObject(typeof(T).Name).AddComponent<T>();

    public virtual void Awake() {
      if (_instance) {
        Destroy(gameObject);
      } else {
        _instance = this as T;
        HandlePersistence();
      }
    }

    protected virtual void HandlePersistence() => DontDestroyOnLoad(gameObject);
  }

  public abstract class MonoBehaviourSingletonNonPersistent<T> : MonoBehaviourSingleton<T> where T : Component {
    protected override void HandlePersistence() { }
  }
}
