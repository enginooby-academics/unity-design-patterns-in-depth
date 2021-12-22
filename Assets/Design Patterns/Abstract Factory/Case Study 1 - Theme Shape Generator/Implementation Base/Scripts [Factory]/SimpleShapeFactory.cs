namespace AbstractFactoryPattern.Case1.Base {
  /// <summary>
  /// * [A 'Concrete Factory']
  /// </summary>
  public class SimpleShapeFactory : ShapeFactory {
    public override Cube CreateCube() => new SimpleCube();
    public override Sphere CreateSphere() => new SimpleSphere();
  }
}