using UnityEngine;

namespace BuilderPattern.Case1.Base {
  // Makes the parts and adds them to the vehicle at the correct local position.
  public class CarBuilder : IVehicleBuilder {
    public CarBuilder() => Vehicle = new Vehicle("Car");

    public Vehicle Vehicle { get; }

    public void BuildFrame() {
      var frame = Vehicle.MakePart(PrimitiveType.Cube, "frame", new Vector3(2, 1, 1), Color.blue);

      Vehicle.AddPart(frame, Vector3.zero);
    }

    public void BuildDoors() {
      var doorScale = new Vector3(1, 1, 0.05f);

      var leftDoor = Vehicle.MakePart(PrimitiveType.Cube, "left door", doorScale, Color.cyan);
      Vehicle.AddPart(leftDoor, new Vector3(0, 0, 0.5f));

      var rightDoor = Vehicle.MakePart(PrimitiveType.Cube, "right door", doorScale, Color.cyan);
      Vehicle.AddPart(rightDoor, new Vector3(0, 0, -0.5f));
    }

    public void BuildEngine() {
      var engine = Vehicle.MakePart(PrimitiveType.Cube, "engine", 0.5f * Vector3.one, Color.gray);
      Vehicle.AddPart(engine, new Vector3(1, -0.25f, 0));
    }

    public void BuildWheels() {
      var wheelScale = new Vector3(0.5f, 0.1f, 0.5f);

      var wheel = Vehicle.MakePart(PrimitiveType.Cylinder, "wheel", wheelScale, Color.black);
      Vehicle.AddPart(wheel, new Vector3(0.75f, -0.5f, 0.5f));
      wheel.transform.Rotate(new Vector3(90, 0, 0));

      wheel = Vehicle.MakePart(PrimitiveType.Cylinder, "wheel", wheelScale, Color.black);
      Vehicle.AddPart(wheel, new Vector3(-0.75f, -0.5f, 0.5f));
      wheel.transform.Rotate(new Vector3(90, 0, 0));

      wheel = Vehicle.MakePart(PrimitiveType.Cylinder, "wheel", wheelScale, Color.black);
      Vehicle.AddPart(wheel, new Vector3(-0.75f, -0.5f, -0.5f));
      wheel.transform.Rotate(new Vector3(90, 0, 0));

      wheel = Vehicle.MakePart(PrimitiveType.Cylinder, "wheel", wheelScale, Color.black);
      Vehicle.AddPart(wheel, new Vector3(0.75f, -0.5f, -0.5f));
      wheel.transform.Rotate(new Vector3(90, 0, 0));
    }
  }
}