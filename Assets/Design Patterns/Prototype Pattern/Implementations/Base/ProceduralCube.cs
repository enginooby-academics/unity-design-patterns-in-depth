using UnityEngine;

namespace Prototype.Base {
  public class ProceduralCube : Prototype.ProceduralCube, ICloneable {
    public ProceduralCube(string name, Color color, Vector3 position, float size = 1) : base(name, color, position, size) {
    }

    public override object Clone(Vector3? newPos) {
      return newPos.HasValue
      ? new ProceduralCube(Name, Color, newPos.Value, _size)
      : new ProceduralCube(Name, Color, Position, _size);
    }

    public object CloneWithColor(Color color) {
      return new ProceduralCube(Name, color, Position, _size);
    }
  }
}
