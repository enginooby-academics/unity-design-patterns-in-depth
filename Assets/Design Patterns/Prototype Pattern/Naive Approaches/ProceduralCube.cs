using UnityEngine;

namespace Prototype.Naive {
  public class ProceduralCube : Prototype.ProceduralCube {
    public ProceduralCube(string name, Color color, Vector3 position, float size = 1) : base(name, color, position, size) {
    }

    public float Size => _size;
  }
}
