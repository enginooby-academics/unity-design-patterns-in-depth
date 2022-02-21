using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Core;
#endif

public class SerializedMonoBehaviourSingleton<T> : SerializedMonoBehaviour where T : Component {
  private static T _instance;

  public static T Instance {
    get {
      if (_instance == null) {
        var objs = FindObjectsOfType(typeof(T)) as T[];
        if (objs.Length > 0)
          _instance = objs[0];
        if (objs.Length > 1) Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
        if (_instance == null) {
          var obj = new GameObject();
          obj.HideAndDontSave();
          _instance = obj.AddComponent<T>();
        }
      }

      return _instance;
    }
  }

  public virtual void Awake() {
    if (_instance) {
      Destroy(gameObject);
    }
    else {
      _instance = this as T;
      DontDestroyOnLoad(gameObject);
    }
  }
}