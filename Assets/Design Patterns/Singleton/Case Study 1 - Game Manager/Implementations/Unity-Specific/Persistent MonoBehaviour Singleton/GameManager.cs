using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// + eager initialization
// + global access
// + scene-persistent
// + unique
namespace Singleton.Persistent {
  public class GameManager : Singleton.GameManager {
    public static GameManager Instance;

    private void Awake() {
      if (Instance) {
        Destroy(gameObject);
      } else {
        Instance = this;
        DontDestroyOnLoad(gameObject); // !
      }
    }
  }
}
