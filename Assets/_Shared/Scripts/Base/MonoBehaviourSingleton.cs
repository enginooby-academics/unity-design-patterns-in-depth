using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component {
  protected static T _instance;
  public static T Instance {
    get {
      if (_instance == null) {
        var objs = FindObjectsOfType(typeof(T)) as T[];
        if (objs.Length > 0)
          _instance = objs[0];
        if (objs.Length > 1) {
          Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
        }
        if (_instance == null) {
          GameObject obj = new GameObject();
          obj.HideAndDontSave();
          _instance = obj.AddComponent<T>();
        }
      }
      return _instance;
    }
  }

  public virtual void Awake() {
    if (DoesInstanceExist) {
      print("Destroy " + gameObject.name);
      Destroy(gameObject);
    } else {
      _instance = this as T;
      DontDestroyOnLoad(gameObject);
      AwakeSingleton();
    }
  }

  protected virtual bool DoesInstanceExist => _instance;

  /// <summary>
  /// Override to add addictional Awake logic for the singleton.
  /// </summary>
  // FIX: sometimes is not called
  public virtual void AwakeSingleton() { }
}


public static class Singleton {
  // ! Cannot use
  public static T Instance<T>() where T : MonoBehaviourSingleton<T> {
    T instance = Object.FindObjectOfType<T>();

    if (instance == null) {
      instance = MonoBehaviourSingleton<T>.Instance;
    }

    return instance;
  }
}