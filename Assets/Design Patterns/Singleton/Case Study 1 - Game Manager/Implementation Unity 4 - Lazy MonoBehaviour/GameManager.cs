using UnityEngine;
using Shared = SingletonPattern.Case1;

// + lazy init
// + scene-persistent
// + unique
namespace SingletonPattern.Case1.Unity4 {
  public class GameManager : Shared.GameManager {
    // ! make instance private 
    private static GameManager _instance;

    // ! provide public property for accessing instance, where we add lazy init logic
    public static GameManager Instance {
      get {
        _instance = _instance ?? FindObjectOfType<GameManager>() ?? CreateNewInstance();
        return _instance;
      }
    }

    private static GameManager CreateNewInstance() {
      var go = new GameObject(nameof(GameManager));
      DontDestroyOnLoad(go);
      return go.AddComponent<GameManager>();
    }

    private void Awake() {
      if (_instance) {
        Destroy(gameObject);
      } else {
        _instance = this;
        DontDestroyOnLoad(gameObject);
      }
    }
  }
}
