namespace VisitorPattern.Case1.Base {
  /// <summary>
  /// * A 'Concrete Visitor' class
  /// </summary>
  public class VolumeCalculator : Calculator {
    public float Calculate(ProceduralCube cube) {
      return 1;
    }

    public float Calculate(ProceduralSphere sphere) {
      return 2;
    }

    public float Calculate(ProceduralCylinder cylinder) {
      return 3;
    }
  }
}
