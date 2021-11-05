using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// + eager initialization
// + global access
// + scene-persistent: no if access variable via static instance
// + unique: no
namespace Singleton.Static {
  public class GameManager : Singleton.GameManager {
    #region Static Implementation
    public static GameManager Instance;

    private void Awake() {
      Instance = this;
    }
    #endregion
  }
}