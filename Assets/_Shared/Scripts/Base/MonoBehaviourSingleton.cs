using UnityEngine;

/// <summary>
///   Don't use OnDisable or OnDestroy for reset singleton data. Use OnApplicationQuit.
/// </summary>
public class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component {
  protected static T _instance;

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

  protected virtual bool DoesInstanceExist => _instance;

  public virtual void Awake() {
    if (DoesInstanceExist) {
      print("Destroy " + gameObject.name);
      Destroy(gameObject);
    }
    else {
      _instance = this as T;
      DontDestroyOnLoad(gameObject);
      AwakeSingleton();
    }
  }

  /// <summary>
  ///   Override to add additional Awake logic for the singleton.
  /// </summary>
  // FIX: sometimes is not called
  public virtual void AwakeSingleton() { }
}


public static class Singleton {
  // ! Cannot use
  public static T Instance<T>() where T : MonoBehaviourSingleton<T> {
    var instance = Object.FindObjectOfType<T>();

    if (instance == null) instance = MonoBehaviourSingleton<T>.Instance;

    return instance;
  }
}