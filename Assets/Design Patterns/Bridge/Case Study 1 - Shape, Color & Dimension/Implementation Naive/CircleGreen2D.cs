using UnityEngine;

namespace BridgePattern.Case1.Naive {
  public class CircleGreen2D : IShape {
    public CircleGreen2D() {
    }

    public void Draw() {
      GameObject shape = GameObject.CreatePrimitive(PrimitiveType.Sphere);
      shape.transform.localScale = new Vector3(1, 1, .01f);
      shape.GetComponent<MeshRenderer>().sharedMaterial.color = Color.green;
    }
  }
}
