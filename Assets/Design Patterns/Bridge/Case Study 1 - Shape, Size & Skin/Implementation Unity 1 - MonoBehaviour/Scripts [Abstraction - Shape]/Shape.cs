using UnityEngine;

namespace BridgePattern.Case1.Unity1 {
  [DisallowMultipleComponent]
  [RequireComponent(typeof(Size), typeof(Skin))]
  public abstract class Shape : MonoBehaviour {
  }
}
