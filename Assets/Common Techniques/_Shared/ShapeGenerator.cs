using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace GOConstruction {
  public enum ShapeType {
    RotatingCube,
    ShakingCube,
    RotatingSphere,
    ShakingSphere,
  }

  public abstract class ShapeGenerator : MonoBehaviour {
    [SerializeField] [EnumToggleButtons] protected ShapeType _shapeType;

    [Button]
    public abstract void CreateShape();
  }
}