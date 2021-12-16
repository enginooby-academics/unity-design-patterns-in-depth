using UnityEngine;

namespace BridgePattern.Case1.Naive {
  public class CircleGreen3D : IShape {
    public CircleGreen3D() {
    }

    public void Draw() {
      GameObject shape = GameObject.CreatePrimitive(PrimitiveType.Sphere);
      shape.transform.localScale = new Vector3(1, 1, 1);
      shape.GetComponent<MeshRenderer>().sharedMaterial.color = Color.green;
    }
  }
}
