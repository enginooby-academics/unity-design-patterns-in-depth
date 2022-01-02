namespace AdapterPattern.Case2.Base1 {
  /// <summary>
  /// * [An 'Adapter' class]
  /// Rule: surface area of a 2D shape is the area
  /// </summary>
  public class AreaToSurfaceAreaAdapterB : AreaToSurfaceAreaAdapter {
    public AreaToSurfaceAreaAdapterB(IArea shape2d) : base(shape2d) { }

    public override double GetSurfaceArea() => _shape2d.GetArea();
  }
}