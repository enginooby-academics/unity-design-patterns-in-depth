using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Singleton.Generic {
  public class Player : MonoBehaviour {
    [ShowInInspector] int? a = null;
    int? b = null;
    int? c = null;
    int? d = 4;
    [Button]
    public void GetCurrentLevel() {
      Debug.Log("I'm on level " + GameManager.Instance.Level);
    }

    public int? test() {
      a ??= c ?? d;
      return a;
    }
  }
}
