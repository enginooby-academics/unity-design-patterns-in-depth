using System.Collections.Generic;
using System.Linq;
using Enginooby.Attribute;
using UnityEngine;

namespace AbstractFactoryPattern.Case1 {
  /// <summary>
  ///   Base generator class containing common product usage code for naive and base implementations.
  /// </summary>
  public class ClientShapeGenerator : MonoBehaviour {
    protected List<Cube> generatedCubes = new List<Cube>();
    protected List<Sphere> generatedSpheres = new List<Sphere>();
    protected Vector3 RandomPos => new Vector3(24, 8, 0).RandomRange();

    [Button]
    public void GetTotalDiagonals() {
      var total = generatedCubes.Sum(cube => cube.GetDiagonal());
      print("Total diagonal of all generated cubes is: " + total);
    }

    [Button]
    public void GetTotalDiameters() {
      var total = generatedSpheres.Sum(sphere => sphere.GetDiameter());
      print("Total diameter of all generated spheres is: " + total);
    }
  }
}