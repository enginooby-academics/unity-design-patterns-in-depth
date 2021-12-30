namespace GOConstruction.Scripting {
  public class ScriptingShapeGenerator : ShapeGenerator {
    public override void CreateShape() {
      IShape shape = _shapeType switch
      {
        ShapeType.RotatingCube => new RotatingCube(),
        ShapeType.RotatingSphere => new RotatingSphere(),
        ShapeType.ShakingCube => new ShakingCube(),
        ShapeType.ShakingSphere => new ShakingSphere(),
        _ => throw new System.ArgumentOutOfRangeException(),
      };

      print("Volume of the generated shape is: " + shape.GetVolume());
    }
  }
}
