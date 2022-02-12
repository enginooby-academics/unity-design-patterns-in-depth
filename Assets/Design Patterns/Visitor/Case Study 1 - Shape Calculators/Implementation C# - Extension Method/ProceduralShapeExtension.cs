namespace VisitorPattern.Case1.CSharp {
  public static class ProceduralShapeExtension {
    // ! Using extension method, we don't need to modify shape class hierarchy at all
    // assume all necessary shape field are exposed
    public static double ProcessCalculation(this ProceduralShape shape, Calculator calculator) =>
      calculator.Calculate(shape);
  }
}