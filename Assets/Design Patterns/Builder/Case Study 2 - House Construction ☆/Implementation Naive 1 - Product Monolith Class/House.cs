using System.Collections;
using UnityEngine;
using static UnityEngine.PrimitiveType;
using static UnityEngine.GameObject;

namespace BuilderPattern.Case2.Naive1 {
  public class House {
    public enum Type { Simple, MultiStorey }
    private Type _type;

    public GameObject Container { get; private set; }

    public House(Type type) {
      _type = type;
      string name = _type == Type.Simple ? "Simple House" : "Multi-Storey House";
      Container = new GameObject(name);
    }

    public void Add(GameObject part) {
      part.transform.SetParent(Container.transform);
    }

    // ? Make speed a field of House class
    public IEnumerator BuildBase(float speed) => _type switch
    {
      Type.Simple => BuildBaseSimple(speed),
      Type.MultiStorey => BuildBaseMultiStorey(speed),
      _ => throw new System.ArgumentOutOfRangeException()
    };


    public IEnumerator BuildRoof(float speed) => _type switch
    {
      Type.Simple => BuildRoofSimple(speed),
      Type.MultiStorey => BuildRoofMultiStorey(speed),
      _ => throw new System.ArgumentOutOfRangeException()
    };

    public IEnumerator BuildDoor(float speed) => _type switch
    {
      Type.Simple => BuildDoorSimple(speed),
      Type.MultiStorey => BuildDoorMultiStorey(speed),
      _ => throw new System.ArgumentOutOfRangeException()
    };

    public IEnumerator BuildWindows(float speed) => _type switch
    {
      Type.Simple => BuildWindowsSimple(speed),
      Type.MultiStorey => BuildWindowsMultiStorey(speed),
      _ => throw new System.ArgumentOutOfRangeException()
    };

    public IEnumerator BuildChimney(float speed) => _type switch
    {
      Type.Simple => BuildChimneySimple(speed),
      Type.MultiStorey => BuildChimneyMultiStorey(speed),
      _ => throw new System.ArgumentOutOfRangeException()
    };

    #region SIMPLE HOUSE ---------------------------------------------------------------------------------------------------------------------------------
    private IEnumerator BuildBaseSimple(float speed) {
      Add(CreatePrimitive(Cube)
        .WithMaterial(Color.yellow)
        .WithPosition(-7.4f, -1.5f, 0f)
        .WithRotation(0f, -48f, 0f)
        .WithScale(5f, 3f, 5));

      yield return new WaitForSeconds(100 / speed);
    }

    public IEnumerator BuildChimneySimple(float speed) {
      Add(CreatePrimitive(Cylinder)
        .WithMaterial(Color.magenta)
        .WithPosition(-8.83f, .35f, -1.14f)
        .WithRotation(0, -48f, 0)
        .WithScale(.8f, 1.7f, .8f));

      yield return new WaitForSeconds(100 / speed);
    }

    public IEnumerator BuildDoorSimple(float speed) {
      Add(CreatePrimitive(Cube)
        .WithMaterial(Color.red)
        .WithPosition(-5.53f, -2.3f, -1.67f)
        .WithRotation(0f, -48f, 0f)
        .WithScale(1.5f, 1.5f, .1f));

      yield return new WaitForSeconds(100 / speed);
    }

    public IEnumerator BuildRoofSimple(float speed) {
      Add(CreatePrimitive(Cube)
        .WithMaterial(Color.red)
        .WithPosition(-6.24f, .46f, 1.29f)
        .WithRotation(0f, 132f, 28f)
        .WithScale(4f, .2f, 5f));

      Add(CreatePrimitive(Cube)
        .WithMaterial(Color.red)
        .WithPosition(-8.56f, .46f, -1.3f)
        .WithRotation(0f, -48f, 28f)
        .WithScale(4f, .2f, 5f));

      yield return new WaitForSeconds(100 / speed);
    }

    public IEnumerator BuildWindowsSimple(float speed) {
      Add(CreatePrimitive(Cube)
        .WithMaterial(Color.blue)
        .WithPosition(-4.5f, -1f, -.52f)
        .WithRotation(0f, -48f, 0f)
        .WithScale(.8f, .8f, .1f));

      Add(CreatePrimitive(Cube)
        .WithMaterial(Color.blue)
        .WithPosition(-6.5f, -1f, -2.78f)
        .WithRotation(0f, -48f, 0f)
        .WithScale(.8f, .8f, .1f));

      yield return new WaitForSeconds(100 / speed);
    }
    #endregion SIMPLE HOUSE ---------------------------------------------------------------------------------------------------------------------------------

    #region MULTI-STOREY HOUSE ---------------------------------------------------------------------------------------------------------------------------------
    public IEnumerator BuildBaseMultiStorey(float speed) {
      Add(CreatePrimitive(Cube)
        .WithMaterial(Color.black)
        .WithPosition(5.8f, .46f, 0f)
        .WithRotation(0f, 45f, 0f)
        .WithScale(5f, 7f, 5));

      yield return new WaitForSeconds(100 / speed);
    }

    public IEnumerator BuildChimneyMultiStorey(float speed) {
      Add(CreatePrimitive(Cylinder)
        .WithMaterial(Color.black)
        .WithPosition(7.7f, 5.4f, 0f)
        .WithRotation(0f, 44f, 0f)
        .WithScale(.8f, 1.37f, .8f));

      yield return new WaitForSeconds(100 / speed);
    }

    public IEnumerator BuildDoorMultiStorey(float speed) {
      Add(CreatePrimitive(Cube)
        .WithMaterial(Color.red)
        .WithPosition(4f, -1.8f, -1.8f)
        .WithRotation(0f, 45f, 0f)
        .WithScale(1.5f, 2.43f, .1f));

      yield return new WaitForSeconds(100 / speed);
    }

    public IEnumerator BuildRoofMultiStorey(float speed) {
      Add(CreatePrimitive(Cube)
        .WithMaterial(Color.white)
        .WithPosition(6.5f, 5.3f, -.7f)
        .WithRotation(0f, 224f, 25f)
        .WithScale(2f, .2f, 5f));

      Add(CreatePrimitive(Cube)
        .WithMaterial(Color.white)
        .WithPosition(7.8f, 3.5f, -2f)
        .WithRotation(0f, 224f, 57f)
        .WithScale(3f, .2f, 5f));

      Add(CreatePrimitive(Cube)
        .WithMaterial(Color.white)
        .WithPosition(5.1f, 5.3f, .7f)
         .WithRotation(0f, 45f, 25f)
        .WithScale(2f, .2f, 5f));

      Add(CreatePrimitive(Cube)
         .WithMaterial(Color.white)
         .WithPosition(3.9f, 3.5f, 1.9f)
         .WithRotation(0f, 45f, 57f)
         .WithScale(3f, .2f, 5f));

      yield return new WaitForSeconds(100 / speed);
    }

    public IEnumerator BuildWindowsMultiStorey(float speed) {
      Add(CreatePrimitive(Cube)
        .WithMaterial(Color.white)
        .WithPosition(4f, .8f, -1.7f)
        .WithRotation(0f, 44f, 0f)
        .WithScale(.74f, .9f, .1f));

      Add(CreatePrimitive(Cube)
        .WithMaterial(Color.white)
        .WithPosition(5f, .8f, -2.9f)
        .WithRotation(0f, 44f, 0f)
        .WithScale(.74f, .9f, .1f));

      Add(CreatePrimitive(Cube)
        .WithMaterial(Color.white)
        .WithPosition(3f, .8f, -.7f)
        .WithRotation(0f, 44f, 0f)
        .WithScale(.74f, .9f, .1f));

      Add(CreatePrimitive(Cube)
        .WithMaterial(Color.white)
        .WithPosition(4.7f, 2.4f, -2.5f)
        .WithRotation(0f, 44f, 0f)
        .WithScale(1.3f, .9f, .1f));

      Add(CreatePrimitive(Cube)
      .WithMaterial(Color.white)
      .WithPosition(3.4f, 2.4f, -1.1f)
      .WithRotation(0f, 44f, 0f)
      .WithScale(1.3f, .9f, .1f));

      yield return new WaitForSeconds(100 / speed);
    }
    #endregion MULTI-STOREY HOUSE ---------------------------------------------------------------------------------------------------------------------------------
  }
}