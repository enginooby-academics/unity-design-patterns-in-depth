using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Reflection.Case1.Naive {
  public class ShapeManager : MonoBehaviour {
    public enum ShapeType {
      Cube,
      Sphere,
      MonoBehaviourCube,
      MonoBehaviourSphere,
    }

    [SerializeField] [EnumToggleButtons] private ShapeType _currentShapeType;

    // [SerializeReference]
    // private Shape _shapeType; // this will immediately create new instance of the selected type

    [Button]
    public void CreateShape() {
      IShape shape = _currentShapeType switch {
        ShapeType.Cube => new Cube(),
        ShapeType.Sphere => new Sphere(),
        ShapeType.MonoBehaviourCube => CreateMonoBehaviourCube(),
        ShapeType.MonoBehaviourSphere => CreateMonoBehaviourCube(),
        _ => throw new ArgumentOutOfRangeException(),
      };

      print("Volume of the newly-created shape is: " + shape.GetVolume());
    }

    private MonoBehaviourCube CreateMonoBehaviourCube() {
      var go = new GameObject();
      go.AddComponent<MeshFilter>();
      go.AddComponent<MeshRenderer>();
      return go.AddComponent<MonoBehaviourCube>();
    }

    private MonoBehaviourSphere CreateMonoBehaviourSphere() {
      var go = new GameObject();
      go.AddComponent<MeshFilter>();
      go.AddComponent<MeshRenderer>();
      return go.AddComponent<MonoBehaviourSphere>();
    }
  }
}