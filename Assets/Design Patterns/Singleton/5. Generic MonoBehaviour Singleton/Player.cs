using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Singleton.Generic {
  public class Player : MonoBehaviour {
    // [Button]
    private void Start() {
      Invoke(nameof(GetCurrentLevel), 3);
    }
    public void GetCurrentLevel() {
      Debug.Log("I'm on level " + GameManager.Instance.Level);
    }
  }
}
