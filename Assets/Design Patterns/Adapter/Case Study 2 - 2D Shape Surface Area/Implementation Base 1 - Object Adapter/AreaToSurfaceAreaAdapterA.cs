namespace AdapterPattern.Case2.Base1 {
  /// <summary>
  /// * [An 'Adapter' class]
  /// Rule: surface area of a 2D shape is the total area of exposed faces = 2 * area
  /// </summary>
  public class AreaToSurfaceAreaAdapterA : AreaToSurfaceAreaAdapter {
    // ! Option 1: tradditional approach, client shouldn't know about adapter
    public AreaToSurfaceAreaAdapterA(IArea shape2d) : base(shape2d) { }

    public override double GetSurfaceArea() => _shape2d.GetArea() * 2;

    // ! Option 2: client knows about adapter
    // public AreaToSurfaceAreaAdapterA() {}

    // public double GetSurfaceArea(IArea shape2d) => shape2d.GetArea() * 2;

    // ! Option 3: utility method
    // public static double GetSurfaceArea(IArea shape2d) => shape2d.GetArea() * 2;
  }

  // ! Option 4: extension method
  public static class Shape2DExtension {
    // public static double GetSurfaceArea(this IArea shape2d) => shape2d.GetArea() * 2;
  }
}