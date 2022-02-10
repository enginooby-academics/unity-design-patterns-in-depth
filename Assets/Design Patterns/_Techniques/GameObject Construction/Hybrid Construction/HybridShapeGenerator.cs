using UnityEngine;

namespace GOConstruction.Hybrid {
  public class HybridShapeGenerator : ShapeGenerator {
    [SerializeField]
    private RotatingCube _rotatingCubePrefab;

    [SerializeField]
    private ShakingCube _shakingCubePrefab;

    [SerializeField]
    private RotatingSphere _rotatingSpherePrefab;

    [SerializeField]
    private ShakingSphere _shakingSpherePrefab;

    // ! Use GUI-constructed GOs (prefabs)
    // public override void CreateShape() {
    //   IShape shape = _shapeType switch
    //   {
    //     ShapeType.RotatingCube => Instantiate(_rotatingCubePrefab),
    //     ShapeType.RotatingSphere => Instantiate(_rotatingSpherePrefab),
    //     ShapeType.ShakingCube => Instantiate(_shakingCubePrefab),
    //     ShapeType.ShakingSphere => Instantiate(_shakingSpherePrefab),
    //     _ => throw new System.ArgumentOutOfRangeException(),
    //   };

    //   print("Volume of the generated shape is: " + shape.GetVolume());
    // }

    // ! Use scripting-constructed GOs
    public override void CreateShape() {
      var go = new GameObject();

      IShape shape = _shapeType switch
      {
        ShapeType.RotatingCube => go.AddComponent<RotatingCube>(),
        ShapeType.RotatingSphere => go.AddComponent<RotatingSphere>(),
        ShapeType.ShakingCube => go.AddComponent<ShakingCube>(),
        ShapeType.ShakingSphere => go.AddComponent<ShakingSphere>(),
        _ => throw new System.ArgumentOutOfRangeException(),
      };

      print("Volume of the generated shape is: " + shape.GetVolume());
    }
  }
}
