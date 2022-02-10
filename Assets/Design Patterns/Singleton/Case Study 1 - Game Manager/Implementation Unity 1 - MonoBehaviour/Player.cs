using UnityEngine;

namespace SingletonPattern.Case1.Unity1 {
  public class Player : MonoBehaviour {
    void Start() {
      Debug.Log("I'm on level " + GameManager.Instance.Level);
    }
  }
}
