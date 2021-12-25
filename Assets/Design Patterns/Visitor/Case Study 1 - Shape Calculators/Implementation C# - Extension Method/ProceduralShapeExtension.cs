using UnityEngine;

namespace VisitorPattern.Case1.CSharp {
  public static class ProceduralShapeExtension {
    // ! Using extension method, we don't need to modify shape class hierarchy at all
    // assume all necessary shape field are exposed
    public static void ProcessCalculation(this ProceduralShape shape, Calculator calculator) {
      float result = calculator.Calculate(shape);
      Debug.Log($"{calculator.GetType().Name}: {result}");
    }
  }
}