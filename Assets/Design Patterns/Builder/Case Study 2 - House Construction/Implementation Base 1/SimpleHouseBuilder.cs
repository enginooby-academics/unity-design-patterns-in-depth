using UnityEngine;
using static UnityEngine.PrimitiveType;
using static UnityEngine.GameObject;

namespace BuilderPattern.Case2.Base1 {
  /// <summary>
  /// * A 'Concrete Builder' class
  /// </summary>
  public class SimpleHouseBuilder : IHouseBuilder {
    private House _house;
    public House House => _house ??= new House("Simple House");

    public void BuildBase() {
      House.Add(CreatePrimitive(Cube)
        .WithMaterial(Color.yellow)
        .WithPosition(-7.4f, -1.5f, 0f)
        .WithRotation(0f, -48f, 0f)
        .WithScale(5f, 3f, 5));
    }

    public void BuildChymney() {
      House.Add(CreatePrimitive(Cylinder)
        .WithMaterial(Color.magenta)
        .WithPosition(-8.83f, .35f, -1.14f)
        .WithRotation(0, -48f, 0)
        .WithScale(.8f, 1.7f, .8f));
    }

    public void BuildDoor() {
      House.Add(CreatePrimitive(Cube)
        .WithMaterial(Color.red)
        .WithPosition(-5.53f, -2.3f, -1.67f)
        .WithRotation(0f, -48f, 0f)
        .WithScale(1.5f, 1.5f, .1f));
    }

    public void BuildRoof() {
      House.Add(CreatePrimitive(Cube)
        .WithMaterial(Color.red)
        .WithPosition(-6.24f, .46f, 1.29f)
        .WithRotation(0f, 132f, 28f)
        .WithScale(4f, .2f, 5f));

      House.Add(CreatePrimitive(Cube)
        .WithMaterial(Color.red)
        .WithPosition(-8.56f, .46f, -1.3f)
        .WithRotation(0f, -48f, 28f)
        .WithScale(4f, .2f, 5f));
    }

    public void BuildWindows() {
      House.Add(CreatePrimitive(Cube)
        .WithMaterial(Color.blue)
        .WithPosition(-4.5f, -1f, -.52f)
        .WithRotation(0f, -48f, 0f)
        .WithScale(.8f, .8f, .1f));

      House.Add(CreatePrimitive(Cube)
        .WithMaterial(Color.blue)
        .WithPosition(-6.5f, -1f, -2.78f)
        .WithRotation(0f, -48f, 0f)
        .WithScale(.8f, .8f, .1f));
    }
  }
}