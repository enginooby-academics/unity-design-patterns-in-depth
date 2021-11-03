using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// + eager initialization
// + global access
// + scene-persistent: no
// + unique
namespace Singleton.Simple {
  public class GameManager : Singleton.GameManager {
    public static GameManager Instance;

    private void Awake() {
      if (Instance) {
        Destroy(gameObject);
      } else {
        Instance = this;
      }
    }
  }
}
