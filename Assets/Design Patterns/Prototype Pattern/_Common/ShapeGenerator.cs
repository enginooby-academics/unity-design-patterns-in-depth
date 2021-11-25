using UnityEngine;
using static RayUtils;

namespace Prototype {
  public abstract class ShapeGenerator : MonoBehaviourSingleton<ShapeGenerator> {
    public Prototype.ProceduralShape template;
    protected Vector3 _mousePos = Vector3.zero;

    void Start() {
      GenerateShapes();
    }

    void Update() {
      if (MouseButton.Right.IsDown() && IsMouseRayHit) {
        _mousePos = MousePosOnRayHit.Value;
        CloneTemplate();
      }
    }

    public abstract void GenerateShapes();
    public abstract void CloneTemplate();

    public void SpawnRandom() { }
  }
}