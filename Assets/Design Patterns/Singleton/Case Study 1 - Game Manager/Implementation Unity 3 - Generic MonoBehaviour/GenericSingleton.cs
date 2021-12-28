using UnityEngine;

// + lazy init
// + scene-persistent
// + unique
// + generic
namespace SingletonPattern.Case1.Unity3 {
  public class GenericSingleton<T> : MonoBehaviour where T : Component {
    private static T instance;

    public static T Instance {
      get => instance ??= FindObjectOfType<T>() ?? CreateNewInstance(); // not working
      // get {
      //   if (instance == null) {
      //     instance = FindObjectOfType<T>();
      //     instance ??= CreateNewInstance();
      //   }
      //   return instance;
      // }
    }

    private static T CreateNewInstance() {
      var go = new GameObject(typeof(T).Name);
      return go.AddComponent<T>();
    }

    public virtual void Awake() {
      if (instance) {
        Destroy(gameObject);
      } else {
        instance = this as T;
        DontDestroyOnLoad(gameObject);
      }
    }
  }
}
