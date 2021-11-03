using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Singleton.Simple {
  public class Player : MonoBehaviour {
    void Start() {
      Debug.Log("I'm on level " + GameManager.Instance.Level);
    }
  }
}
