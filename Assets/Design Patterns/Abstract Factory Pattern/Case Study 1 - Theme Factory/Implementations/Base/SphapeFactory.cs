using UnityEngine;

namespace Factory.Base {
  /// <summary>
  /// To add more factory themes, extend this class and implement abstract methods.
  /// </summary>
  public abstract class ShapeFactory : MonoBehaviour {
    [field: SerializeField]
    protected GameObject CubePrefab { get; set; }

    [field: SerializeField]
    protected GameObject SpherePrefab { get; set; }

    [field: SerializeField]
    protected GameObject CylinderPrefab { get; set; }

    public abstract GameObject SpawnCube();
    public abstract GameObject SpawnCylinder();
    public abstract GameObject SpawnSphere();
  }
}