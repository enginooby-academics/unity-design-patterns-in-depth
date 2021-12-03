using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// + lazy initialization
// + global access
// + scene-persistent
// + unique
// + generic
namespace Singleton.Generic {
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
