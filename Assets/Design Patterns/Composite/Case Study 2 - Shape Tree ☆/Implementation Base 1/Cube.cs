using UnityEngine;
using static UnityEngine.Mathf;

namespace CompositePattern.Case2.Base1 {
  /// <summary>
  /// * A 'Leaf' class
  /// </summary>
  public class Cube : Shape {
    // ! Constructor has to be public for [SerializeReference]
    public Cube() : base(PrimitiveType.Cube) { }

    public override double GetVolume() => Pow(_scale, 3);
  }
}
