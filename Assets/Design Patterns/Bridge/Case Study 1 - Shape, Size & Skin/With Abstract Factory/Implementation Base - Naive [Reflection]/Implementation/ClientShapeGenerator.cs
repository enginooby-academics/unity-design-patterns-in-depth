using UnityEngine;
using Sirenix.OdinInspector;

namespace BridgePattern.Case1.Base.WithAbstractFactory.Base {
  /// <summary>
  /// * [The 'Client' class]
  /// </summary>
  public class ClientShapeGenerator : MonoBehaviour {
    [SerializeReference]
    private ShapeFactory _currentShapeFactory;

    // ! Hide constraint logic from the client
    [Button]
    public void GenerateShape() => _currentShapeFactory.GetShape();
  }
}
