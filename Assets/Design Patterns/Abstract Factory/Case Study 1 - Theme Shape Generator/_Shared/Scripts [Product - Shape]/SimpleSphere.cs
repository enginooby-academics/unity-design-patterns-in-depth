using UnityEngine;

namespace AbstractFactoryPattern.Case1 {
  /// <summary>
  ///   * [A 'Concrete Product']
  /// </summary>
  public class SimpleSphere : Sphere {
    public SimpleSphere() : base(Random.Range(3f, 4f), Color.red) {
    }
  }
}