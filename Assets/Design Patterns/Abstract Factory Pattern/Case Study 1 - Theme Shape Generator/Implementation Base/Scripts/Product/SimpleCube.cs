using UnityEngine;

namespace AbstractFactoryPattern.Case1.Base {
  /// <summary>
  /// * [A 'Concrete Product']
  /// </summary>
  public class SimpleCube : Cube {
    public SimpleCube() : base(Random.Range(3f, 4f), Color.red) { }
  }
}
