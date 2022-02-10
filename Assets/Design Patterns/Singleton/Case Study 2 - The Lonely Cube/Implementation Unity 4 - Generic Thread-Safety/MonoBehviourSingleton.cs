using System;
using UnityEngine;

// TODO: Move to Library
// + unique
// + persistent
// + generic
// + thread-safety

namespace SingletonPattern.Case2.Unity4 {
  public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component {
    public static readonly Lazy<T> Instance = new Lazy<T>(MakeInstance);

    private static T MakeInstance() {
      var component = FindObjectOfType<T>();
      if (component != null) return component;

      var go = new GameObject(typeof(T).Name);
      DontDestroyOnLoad(go);
      return go.AddComponent<T>();
    }

    public virtual void Awake() {
      if (Instance.Value) {
        Destroy(gameObject); // ! not destroy
      } else {
        DontDestroyOnLoad(gameObject);
      }
    }
  }
}
