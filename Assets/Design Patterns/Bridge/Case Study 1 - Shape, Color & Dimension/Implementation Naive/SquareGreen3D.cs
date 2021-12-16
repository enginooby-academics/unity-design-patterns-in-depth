using UnityEngine;

namespace BridgePattern.Case1.Naive {
  public class SquareGreen3D : IShape {
    public SquareGreen3D() {
    }

    public void Draw() {
      GameObject shape = GameObject.CreatePrimitive(PrimitiveType.Cube);
      shape.transform.localScale = new Vector3(1, 1, 1f);
      shape.GetComponent<MeshRenderer>().sharedMaterial.color = Color.green;
    }
  }
}
