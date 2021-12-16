using UnityEngine;

namespace BridgePattern.Case1.Naive {
  public class SquareRed2D : IShape {
    public SquareRed2D() {
    }

    public void Draw() {
      GameObject shape = GameObject.CreatePrimitive(PrimitiveType.Cube);
      shape.transform.localScale = new Vector3(1, 1, .01f);
      shape.GetComponent<MeshRenderer>().sharedMaterial.color = Color.red;
    }
  }
}
