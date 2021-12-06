namespace VisitorPattern.Case1.Base {
  /// <summary>
  /// * A 'Concrete Visitor' class
  /// </summary>
  public class SufaceCalculator : Calculator {
    public float Calculate(ProceduralCube cube) {
      return 100;
    }

    public float Calculate(ProceduralSphere sphere) {
      return 200;
    }

    public float Calculate(ProceduralCylinder cylinder) {
      return 300;
    }
  }
}
