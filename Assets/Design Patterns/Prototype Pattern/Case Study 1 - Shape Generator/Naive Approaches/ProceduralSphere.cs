using UnityEngine;

namespace Prototype.Naive {
  public class ProceduralSphere : Prototype.ProceduralSphere {
    public ProceduralSphere(string name, Color color, Vector3 position, float radius = 1, int horizontalSegments = 16, int verticalSegments = 16) : base(name, color, position, radius, horizontalSegments, verticalSegments) {
    }

    public float Radius => _radius;
    public int HorizontalSegments => _horizontalSegments;
    public int VerticalSegments => _verticalSegments;
  }
}
