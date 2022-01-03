using System.Collections;
using UnityEngine;
using static UnityEngine.PrimitiveType;
using static UnityEngine.GameObject;

namespace BuilderPattern.Case2.Base1 {
  /// <summary>
  /// * A 'Concrete Builder' class
  /// </summary>
  public class MultiStoreyHouseBuilder : IHouseBuilder {
    private House _house;
    public House House => _house ??= new House("Multi-Storey House");

    public IEnumerator BuildBase(float speed) {
      House.Add(CreatePrimitive(Cube)
        .WithMaterial(Color.black)
        .WithPosition(5.8f, .46f, 0f)
        .WithRotation(0f, 45f, 0f)
        .WithScale(5f, 7f, 5));

      yield return new WaitForSeconds(100 / speed);
    }

    public IEnumerator BuildChimney(float speed) {
      House.Add(CreatePrimitive(Cylinder)
        .WithMaterial(Color.black)
        .WithPosition(7.7f, 5.4f, 0f)
        .WithRotation(0f, 44f, 0f)
        .WithScale(.8f, 1.37f, .8f));

      yield return new WaitForSeconds(100 / speed);
    }

    public IEnumerator BuildDoor(float speed) {
      House.Add(CreatePrimitive(Cube)
        .WithMaterial(Color.red)
        .WithPosition(4f, -1.8f, -1.8f)
        .WithRotation(0f, 45f, 0f)
        .WithScale(1.5f, 2.43f, .1f));

      yield return new WaitForSeconds(100 / speed);
    }

    public IEnumerator BuildRoof(float speed) {
      House.Add(CreatePrimitive(Cube)
        .WithMaterial(Color.white)
        .WithPosition(6.5f, 5.3f, -.7f)
        .WithRotation(0f, 224f, 25f)
        .WithScale(2f, .2f, 5f));

      House.Add(CreatePrimitive(Cube)
        .WithMaterial(Color.white)
        .WithPosition(7.8f, 3.5f, -2f)
        .WithRotation(0f, 224f, 57f)
        .WithScale(3f, .2f, 5f));

      House.Add(CreatePrimitive(Cube)
        .WithMaterial(Color.white)
        .WithPosition(5.1f, 5.3f, .7f)
         .WithRotation(0f, 45f, 25f)
        .WithScale(2f, .2f, 5f));

      House.Add(CreatePrimitive(Cube)
         .WithMaterial(Color.white)
         .WithPosition(3.9f, 3.5f, 1.9f)
         .WithRotation(0f, 45f, 57f)
         .WithScale(3f, .2f, 5f));

      yield return new WaitForSeconds(100 / speed);
    }

    public IEnumerator BuildWindows(float speed) {
      House.Add(CreatePrimitive(Cube)
        .WithMaterial(Color.white)
        .WithPosition(4f, .8f, -1.7f)
        .WithRotation(0f, 44f, 0f)
        .WithScale(.74f, .9f, .1f));

      House.Add(CreatePrimitive(Cube)
        .WithMaterial(Color.white)
        .WithPosition(5f, .8f, -2.9f)
        .WithRotation(0f, 44f, 0f)
        .WithScale(.74f, .9f, .1f));

      House.Add(CreatePrimitive(Cube)
        .WithMaterial(Color.white)
        .WithPosition(3f, .8f, -.7f)
        .WithRotation(0f, 44f, 0f)
        .WithScale(.74f, .9f, .1f));

      House.Add(CreatePrimitive(Cube)
        .WithMaterial(Color.white)
        .WithPosition(4.7f, 2.4f, -2.5f)
        .WithRotation(0f, 44f, 0f)
        .WithScale(1.3f, .9f, .1f));

      House.Add(CreatePrimitive(Cube)
      .WithMaterial(Color.white)
      .WithPosition(3.4f, 2.4f, -1.1f)
      .WithRotation(0f, 44f, 0f)
      .WithScale(1.3f, .9f, .1f));

      yield return new WaitForSeconds(100 / speed);
    }
  }
}