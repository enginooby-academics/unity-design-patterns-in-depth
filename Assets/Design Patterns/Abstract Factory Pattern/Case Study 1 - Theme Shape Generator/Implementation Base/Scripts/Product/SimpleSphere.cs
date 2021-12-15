using UnityEngine;

namespace AbstractFactoryPattern.Case1.Base {
  public class SimpleSphere : Sphere {
    /// * [A 'Concrete Product']
    public SimpleSphere() : base(Random.Range(3f, 4f), Color.red) { }
  }
}
