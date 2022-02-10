using UnityEngine;

namespace SingletonPattern.Case1.Unity3 {
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
