namespace AdapterPattern.Case2.Base1 {
  /// <summary>
  /// * [An 'Adapter' class]
  /// Rule: surface area of a 2D shape is the total area of exposed faces = area * 2
  /// </summary>
  public class AreaToSurfaceAreaAdapterA : AreaToSurfaceAreaAdapter {
    public override double GetSurfaceArea() => _shape2d.Result.GetArea() * 2;
  }
}