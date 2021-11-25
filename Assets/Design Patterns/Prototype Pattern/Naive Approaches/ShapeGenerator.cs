using UnityEngine;
using static VectorUtils;
using static QuaternionUtils;
using static RayUtils;

namespace Prototype.Naive {
  public class ShapeGenerator : MonoBehaviourSingleton<ShapeGenerator> {
    public ProceduralShape template;
    private Vector3 _mousePos = Vector3.zero;

    void Start() {
      var cube = new ProceduralCube("Cube", Color.green, v0, q0, v1, size: 1f);
    }

    public void CloneTemplate() {
      if (Physics.Raycast(MouseRay, out var hit)) {
        _mousePos = hit.point;
      }
      if (template is ProceduralCube) {
        var templateCube = template as ProceduralCube;
        var clone = new ProceduralCube(templateCube.Name, Color.black, _mousePos, q0, v1, templateCube.Size);
      }
    }

    public void SpawnRandom() {

    }

    void Update() {
      if (MouseButton.Right.IsDown()) {
        CloneTemplate();
      }
    }
  }
}