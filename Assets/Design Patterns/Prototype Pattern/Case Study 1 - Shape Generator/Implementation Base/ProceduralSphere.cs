using UnityEngine;

namespace Prototype.Base {
  public class ProceduralSphere : Prototype.ProceduralSphere, ICloneable {
    public ProceduralSphere(string name, Color color, Vector3 position, float radius = 1, int horizontalSegments = 16, int verticalSegments = 16) : base(name, color, position, radius, horizontalSegments, verticalSegments) {
    }

    public override object Clone(Vector3? newPos) {
      return newPos.HasValue
      ? new ProceduralSphere(Name, Color, newPos.Value, _radius, _horizontalSegments, _verticalSegments)
      : new ProceduralSphere(Name, Color, Position, _radius, _horizontalSegments, _verticalSegments);
    }

    public object CloneWithColor(Color color) {
      return new ProceduralSphere(Name, color, Position, _radius, _horizontalSegments, _verticalSegments);
    }
  }
}
