using Sirenix.OdinInspector;
using UnityEngine;

namespace GOConstruction {
  public enum ShapeType { RotatingCube, ShakingCube, RotatingSphere, ShakingSphere }

  public abstract class ShapeGenerator : MonoBehaviour {
    [SerializeField, EnumToggleButtons]
    protected ShapeType _shapeType;

    [Button] public abstract void CreateShape();
  }
}
