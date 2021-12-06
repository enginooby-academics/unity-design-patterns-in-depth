namespace VisitorPattern.Case1.Base {
  /// <summary>
  /// * A 'Concrete Visitor' class
  /// </summary>
  public class AreaCalculator : Calculator {
    public float Calculate(ProceduralCube cube) {
      return 10;
    }

    public float Calculate(ProceduralSphere sphere) {
      return 20;
    }

    public float Calculate(ProceduralCylinder cylinder) {
      return 30;
    }
  }
}
