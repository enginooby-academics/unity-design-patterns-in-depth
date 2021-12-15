using UnityEngine;

namespace AbstractFactoryPattern.Case1.Base {
  public class SimpleCube : Cube {
    /// * [A 'Concrete Product']
    public SimpleCube() : base(Random.Range(3f, 4f), Color.red) { }
  }
}
