namespace AbstractFactoryPattern.Case1.Base {
  /// <summary>
  ///   * [The 'Abstract Factory']
  /// </summary>
  public abstract class ShapeFactory {
    public abstract Cube CreateCube();
    public abstract Sphere CreateSphere();
  }
}