using UnityEngine;
using static VectorUtils;

namespace Prototype.Naive {
  public class ShapeGenerator : Prototype.ShapeGenerator {
    public override void GenerateShapes() {
      // ! Initialize similiar instances repeatedly by long constructors
      var cube1 = new ProceduralCube("Cube", Color.green, v0, size: 1f);
      var cube2 = new ProceduralCube("Cube", Color.red, v0, size: 1f);
      var cube3 = new ProceduralCube("Cube", Color.yellow, v0, size: 1f);
      var sphere1 = new ProceduralSphere("Sphere", Color.green, v0, radius: 1f, horizontalSegments: 16, verticalSegments: 16);
      var sphere2 = new ProceduralSphere("Sphere", Color.red, v0, radius: 1f, horizontalSegments: 16, verticalSegments: 16);
      var sphere3 = new ProceduralSphere("Sphere", Color.yellow, v0, radius: 1f, horizontalSegments: 16, verticalSegments: 16);
      var cylinder1 = new ProceduralCylinder("Cylinder", Color.green, v0, radius: 1f, segments: 16, height: 1);
      var cylinder2 = new ProceduralCylinder("Cylinder", Color.red, v0, radius: 1f, segments: 16, height: 1);
      var cylinder3 = new ProceduralCylinder("Cylinder", Color.yellow, v0, radius: 1f, segments: 16, height: 1);
      template = cube1;
    }

    public override void CloneTemplate() {
      // ! To clone an existing instance, we need to know its concrete type then retrieve all property values of the instance.
      if (template is ProceduralCube) {
        var templateCube = template as ProceduralCube;
        var clone = new ProceduralCube(templateCube.Name, templateCube.Color, _mousePos, templateCube.Size);
      } else if (template is ProceduralSphere) {
        var templateSphere = template as ProceduralSphere;
        var clone = new ProceduralSphere(templateSphere.Name, templateSphere.Color, _mousePos, templateSphere.Radius, templateSphere.HorizontalSegments, templateSphere.VerticalSegments);
      } else if (template is ProceduralCylinder) {
        var templateCylinder = template as ProceduralCylinder;
        var clone = new ProceduralCylinder(templateCylinder.Name, templateCylinder.Color, _mousePos, templateCylinder.Radius, templateCylinder.Segments, templateCylinder.Height);
      }
    }
  }
}