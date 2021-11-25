using UnityEngine;

namespace Prototype.Naive {
  public class ProceduralCube : ProceduralShape {
    private float _size;
    public float Size => _size;

    public ProceduralCube(string name, Color color, Vector3 position, Quaternion rotation, Vector3 localScale, float size = 1f)
    : base(name, color, position, rotation, localScale) {
      _size = size;
      CreateMesh();
      _gameObject.AddComponent<BoxCollider>();
    }

    protected override void CreateMeshData() {
      Vector3 width = _size * Vector3.right;
      Vector3 length = _size * Vector3.forward;
      Vector3 height = _size * Vector3.up;

      Vector3 v000 = -width / 2 - length / 2 - height / 2;
      Vector3 v001 = v000 + height;
      Vector3 v010 = v000 + width;
      Vector3 v011 = v000 + width + height;
      Vector3 v100 = v000 + length;
      Vector3 v101 = v000 + length + height;
      Vector3 v110 = v000 + width + length;
      Vector3 v111 = v000 + width + length + height;

      Vector2 uv0 = new Vector2(0, 0);
      Vector2 uv1 = new Vector2(0, 1);
      Vector2 uv2 = new Vector2(1, 1);
      Vector2 uv3 = new Vector2(1, 0);

      AddQuadUV(v100, v101, v001, v000, Vector3.left, uv0, uv1, uv2, uv3);
      AddQuadUV(v010, v011, v111, v110, Vector3.right, uv0, uv1, uv2, uv3);
      AddQuadUV(v010, v110, v100, v000, Vector3.down, uv0, uv1, uv2, uv3);
      AddQuadUV(v111, v011, v001, v101, Vector3.up, uv0, uv1, uv2, uv3);
      AddQuadUV(v000, v001, v011, v010, Vector3.back, uv0, uv1, uv2, uv3);
      AddQuadUV(v110, v111, v101, v100, Vector3.forward, uv0, uv1, uv2, uv3);
    }

    public override void OnUpdate() {
      // _shapeMonoBehaviour.transform.Rotate(Vector3.up * Time.deltaTime * 10f);
    }

  }
}
