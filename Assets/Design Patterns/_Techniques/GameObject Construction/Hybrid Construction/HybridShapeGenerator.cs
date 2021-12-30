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

    public override void CreateShape() {
      IShape shape = _shapeType switch
      {
        ShapeType.RotatingCube => Instantiate(_rotatingCubePrefab),
        ShapeType.RotatingSphere => Instantiate(_rotatingSpherePrefab),
        ShapeType.ShakingCube => Instantiate(_shakingCubePrefab),
        ShapeType.ShakingSphere => Instantiate(_shakingSpherePrefab),
        _ => throw new System.ArgumentOutOfRangeException(),
      };

      print("Volume of the generated shape is: " + shape.GetVolume());
    }
  }
}
