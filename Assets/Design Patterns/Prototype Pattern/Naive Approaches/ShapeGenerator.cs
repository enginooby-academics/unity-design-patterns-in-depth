using UnityEngine;
using static VectorUtils;
using static RayUtils;

namespace Prototype.Naive {
  public class ShapeGenerator : MonoBehaviourSingleton<ShapeGenerator> {
    public ProceduralShape template;
    private Vector3 _mousePos = Vector3.zero;

    void Start() {
      var greenCube = new ProceduralCube("Cube", Color.green, v0, size: 1f);
      var redCube = new ProceduralCube("Cube", Color.red, v0, size: 1f);
      var sphere = new ProceduralSphere("Sphere", Color.yellow, v0);
      template = greenCube;
    }

    public void CloneTemplate() {
      if (template is ProceduralCube) {
        var templateCube = template as ProceduralCube;
        var clone = new ProceduralCube(templateCube.Name, templateCube.Color, _mousePos, templateCube.Size);
      } else if (template is ProceduralSphere) {
        var templateSphere = template as ProceduralSphere;
        print(templateSphere.Name);
        var clone = new ProceduralSphere(templateSphere.Name, templateSphere.Color, _mousePos, templateSphere.Radius);
      }
    }

    public void SpawnRandom() {

    }

    void Update() {
      if (MouseButton.Right.IsDown() && IsMouseRayHit) {
        _mousePos = MousePosOnRayHit.Value;
        CloneTemplate();
      }
    }
  }
}