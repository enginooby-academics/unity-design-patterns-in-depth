using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VectorUtils;

namespace Prototype.Naive {
  public class ProceduralCube : ProceduralShape {
    public ProceduralCube(string name, Color color, Vector3 position, Quaternion rotation, Vector3 localScale)
    : base(name, color, position, rotation, localScale) {
      CreateMesh();
      gameObject.AddComponent<BoxCollider>();
    }

    public override void OnUpdate() {
    }

    protected override void CreateMeshData() {
      vertices = new Vector3[]{
        v000, v100, v110, v010,
        v011, v111, v101, v001,
    };

      triangles = new int[]{
        0, 2, 1, 0, 3, 2, // front face
        2, 3, 4, 2, 4, 5, // top
        1, 2, 5, 1, 5, 6, // right
        0, 7, 4, 0, 4, 3, // left
        5, 4, 7, 5, 7, 6, // back
        0, 6, 7, 0, 1, 6, // bottom
    };
    }
  }
}
