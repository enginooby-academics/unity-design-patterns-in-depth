using UnityEngine;
using System;

// + lazy init
// + scene-persistent ?
// + unique
// + generic
// + thread-safety

namespace SingletonPattern.Case1.Unity5 {
  public class GenericSingleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static readonly Lazy<T> _instance = new Lazy<T>(CreateNewInstance);

    public static T Instance => _instance.Value;

    private static T CreateNewInstance() {
      var go = new GameObject(typeof(T).Name);
      var instance = go.AddComponent<T>();
      DontDestroyOnLoad(go);
      return instance;
    }

    public virtual void Awake() {
      // if (_instance.Value != null) {
      //   Destroy(gameObject);
      // } else {
      //   // _instance.Value = this as T;
      //   // DontDestroyOnLoad(gameObject);
      // }
    }
  }
}
