using UnityEngine;
using Sirenix.OdinInspector;

namespace SingletonPattern.Case1.Unity4 {
  public class Player : MonoBehaviour {
    [Button]
    public void GetCurrentLevel() {
      Debug.Log("I'm on level " + GameManager.Instance.Level);
    }
  }
}
