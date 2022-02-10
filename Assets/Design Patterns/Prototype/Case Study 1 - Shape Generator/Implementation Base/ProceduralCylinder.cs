using UnityEngine;

namespace Prototype.Base {
  public class ProceduralCylinder : Prototype.ProceduralCylinder, ICloneable {
    public ProceduralCylinder(string name, Color color, Vector3 position, float radius = 1, int segments = 16, float height = 1) : base(name, color, position, radius, segments, height) {
    }

    public override object Clone(Vector3? newPos) {
      return newPos.HasValue
      ? new ProceduralCylinder(Name, Color, newPos.Value, _radius, _segments, _height)
      : new ProceduralCylinder(Name, Color, Position, _radius, _segments, _height);
    }

    public object CloneWithColor(Color color) {
      return new ProceduralCylinder(Name, color, Position, _radius, _segments, _height);
    }
  }
}
