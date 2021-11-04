using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Singleton.Lazy {
  public class Player : MonoBehaviour {
    [Button]
    public void GetCurrentLevel() {
      Debug.Log("I'm on level " + GameManager.Instance.Level);
    }
  }
}
