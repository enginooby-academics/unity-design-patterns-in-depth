using UnityEngine;

namespace Prototype.Naive {
  public abstract class ProceduralShape : Prototype.ProceduralShape {
    protected ProceduralShape(string name, Color color, Vector3 position) : base(name, color, position) {
    }
  }
}
