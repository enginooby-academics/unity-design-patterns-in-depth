using UnityEngine;
using static VectorUtils;

namespace Prototype.Unity.Instantiate {
  public class ShapeGenerator : Prototype.ShapeGenerator {
    public GameObject templateGameObject;

    public override void GenerateShapes() {
      var cube1 = new ProceduralCube("Cube", Color.green, v0, 1f);
      var cube2GameObject = Instantiate(cube1.GameObject).AddComponent<ShapeMonoBehaviour>().gameObject;
      var cube3GameObject = Instantiate(cube1.GameObject).AddComponent<ShapeMonoBehaviour>().gameObject;

      var sphere1 = new ProceduralSphere("Sphere", Color.red, v0, 1f, 16, 16);
      var sphere2GameObject = Instantiate(sphere1.GameObject).AddComponent<ShapeMonoBehaviour>().gameObject;
      var sphere3GameObject = Instantiate(sphere1.GameObject).AddComponent<ShapeMonoBehaviour>().gameObject;

      var cylinder1 = new ProceduralCylinder("Cylinder", Color.yellow, v0, 1f, 16, 1);
      var cylinder2GameObject = Instantiate(cylinder1.GameObject).AddComponent<ShapeMonoBehaviour>().gameObject;
      var cylinder3GameObject = Instantiate(cylinder1.GameObject).AddComponent<ShapeMonoBehaviour>().gameObject;

      templateGameObject = cube1.GameObject;
    }

    public override void CloneTemplate() {
      Instantiate(templateGameObject, _mousePos, Quaternion.identity);
    }
  }
}