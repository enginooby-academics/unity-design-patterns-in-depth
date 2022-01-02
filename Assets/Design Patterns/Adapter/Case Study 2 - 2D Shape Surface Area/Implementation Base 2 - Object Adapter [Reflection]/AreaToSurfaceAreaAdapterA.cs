namespace AdapterPattern.Case2.Base2 {
  /// <summary>
  /// * [An 'Adapter' class]
  /// Rule: surface area of a 2D shape is the total area of exposed faces = area * 2
  /// </summary>
  public class AreaToSurfaceAreaAdapterA : AreaToSurfaceAreaAdapter {
    // ! Option 1: pass adaptee to constructor
    public AreaToSurfaceAreaAdapterA(IArea shape2d) : base(shape2d) { }

    public override double GetSurfaceArea() => _shape2d.GetArea() * 2;

    // ! Option 2: pass adaptee to method
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