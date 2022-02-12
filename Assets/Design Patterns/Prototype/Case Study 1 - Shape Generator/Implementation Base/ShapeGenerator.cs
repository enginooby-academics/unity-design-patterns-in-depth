using UnityEngine;
using static VectorUtils;

namespace Prototype.Base {
  public class ShapeGenerator : Prototype.ShapeGenerator {
    public override void GenerateShapes() {
      var cube1 = new ProceduralCube("Cube", Color.green, v0, 1f);
      var cube2 = cube1.CloneWithColor(Color.red);
      var cube3 = cube1.CloneWithColor(Color.yellow);
      var sphere1 = new ProceduralSphere("Sphere", Color.green, v0, 1f, 16, 16);
      var sphere2 = sphere1.CloneWithColor(Color.red);
      var sphere3 = sphere1.CloneWithColor(Color.yellow);
      var cylinder1 = new ProceduralCylinder("Cylinder", Color.green, v0, 1f, 16, 1);
      var cylinder2 = cylinder1.CloneWithColor(Color.red);
      var cylinder3 = cylinder1.CloneWithColor(Color.yellow);
      template = cube1;
    }

    public override void CloneTemplate() {
      template.Clone(_mousePos);
    }
  }
}