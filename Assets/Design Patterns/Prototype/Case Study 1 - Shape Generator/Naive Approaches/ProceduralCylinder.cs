using UnityEngine;

namespace Prototype.Naive {
  public class ProceduralCylinder : Prototype.ProceduralCylinder {
    public ProceduralCylinder(string name, Color color, Vector3 position, float radius = 1, int segments = 16,
      float height = 1) : base(name, color, position, radius, segments, height) {
    }

    public float Radius => _radius;
    public int Segments => _segments;
    public float Height => _height;
  }
}