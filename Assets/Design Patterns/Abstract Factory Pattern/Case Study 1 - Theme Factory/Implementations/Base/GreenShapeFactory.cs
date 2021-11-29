using UnityEngine;

namespace Factory.Base {
  public class GreenShapeFactory : ShapeFactory {
    public override GameObject SpawnCube() {
      return Instantiate(CubePrefab);
    }

    public override GameObject SpawnSphere() {
      return Instantiate(SpherePrefab);
    }

    public override GameObject SpawnCylinder() {
      return Instantiate(CylinderPrefab);
    }
  }
}