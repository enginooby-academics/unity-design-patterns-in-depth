using UnityEngine;

namespace BridgePattern.Case1.Unity2 {
  [DisallowMultipleComponent]
  public abstract class Shape : MonoBehaviour {
    [SerializeField]
    private ReferenceConcreteType<Size> _sizeType;

    [SerializeField]
    private ReferenceConcreteType<Skin> _skinType;

    void Start() {
      gameObject.AddComponent(_sizeType.Value);
      gameObject.AddComponent(_skinType.Value);
    }
  }
}
