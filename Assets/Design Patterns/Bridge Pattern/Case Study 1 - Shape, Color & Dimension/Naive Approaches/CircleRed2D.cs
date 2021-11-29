using UnityEngine;

namespace BridgePattern.Case1.Naive {
  public class CircleRed2D : IShape {
    public CircleRed2D() {
    }

    public void Draw() {
      GameObject shape = GameObject.CreatePrimitive(PrimitiveType.Sphere);
      shape.transform.localScale = new Vector3(1, 1, .01f);
      shape.GetComponent<MeshRenderer>().sharedMaterial.color = Color.red;
    }
  }
}
