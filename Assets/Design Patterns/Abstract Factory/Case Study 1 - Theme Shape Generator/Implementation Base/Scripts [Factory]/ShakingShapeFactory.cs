namespace AbstractFactoryPattern.Case1.Base {
  /// <summary>
  /// * [A 'Concrete Factory']
  /// </summary>
  public class ShakingShapeFactory : ShapeFactory {
    public override Cube CreateCube() => new ShakingCube();
    public override Sphere CreateSphere() => new ShakingSphere();
  }
}