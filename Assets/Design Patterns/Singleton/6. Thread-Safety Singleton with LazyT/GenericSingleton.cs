using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// + lazy initialization
// + global access
// + scene-persistent ?
// + unique: no
// + generic
// + thread-safety
namespace Singleton.ThreadSafety {
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
