using UnityEngine;

namespace GOConstruction.GUI {
  public class GUIShapeGenerator : ShapeGenerator {
    [SerializeField]
    private Cube _rotatingCubePrefab;

    [SerializeField]
    private Cube _shakingCubePrefab;

    [SerializeField]
    private Sphere _rotatingSpherePrefab;

    [SerializeField]
    private Sphere _shakingSpherePrefab;

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
