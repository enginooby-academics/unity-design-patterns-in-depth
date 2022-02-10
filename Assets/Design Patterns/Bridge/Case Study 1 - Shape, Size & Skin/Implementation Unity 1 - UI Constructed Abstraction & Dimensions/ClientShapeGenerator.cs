using Sirenix.OdinInspector;
using UnityEngine;

namespace BridgePattern.Case1.Unity1 {
  public class ClientShapeGenerator : MonoBehaviour {
    [SerializeField]
    private Shape _shapePrefab;

    [Button]
    public void GenerateShape() => Instantiate(_shapePrefab);
  }
}
