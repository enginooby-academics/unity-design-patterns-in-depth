using UnityEngine;
using static RayUtils;

namespace Prototype {
  public abstract class ShapeGenerator : MonoBehaviour {
    protected Vector3 _mousePos = Vector3.zero;
    public ProceduralShape template;

    private void Start() {
      GenerateShapes();
    }

    private void Update() {
      if (MouseButton.Right.IsDown() && IsMouseRayHit) {
        _mousePos = MousePosOnRayHit.Value;
        CloneTemplate();
      }
    }

    public abstract void GenerateShapes();
    public abstract void CloneTemplate();

    public void SpawnRandom() {
    }
  }
}