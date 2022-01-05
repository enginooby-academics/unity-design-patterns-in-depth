using Sirenix.OdinInspector;
using UnityEngine;

namespace BridgePattern.Case1.Unity2 {
  public class ClientShapeGenerator : MonoBehaviour {
    [SerializeField, InlineEditor]
    private Shape _shapePrefab;

    [Button]
    public void GenerateShape() => Instantiate(_shapePrefab);
  }
}
