using UnityEngine;

namespace BuilderPattern.Case1.Base {
  public class ScooterBuilder : IVehicleBuilder {
    public ScooterBuilder() => Vehicle = new Vehicle("Scooter");

    public Vehicle Vehicle { get; }

    public void BuildDoors() {
      // We don't need no doors.
    }

    public void BuildEngine() {
      var engine = Vehicle.MakePart(PrimitiveType.Cube, "engine", 0.5f * new Vector3(.5f, 0.5f, 0.25f), Color.gray);
      Vehicle.AddPart(engine, new Vector3(1.25f, 0.3f, 0));
    }

    public void BuildFrame() {
      var frame = Vehicle.MakePart(PrimitiveType.Cube, "frame", 0.5f * new Vector3(2, 0.5f, 0.5f), Color.magenta);
      Vehicle.AddPart(frame, Vector3.zero);

      frame = Vehicle.MakePart(PrimitiveType.Cube, "frame2", 0.5f * new Vector3(0.25f, 2f, 0.5f), Color.magenta);
      Vehicle.AddPart(frame, new Vector3(1f, 0.75f, 0f));
    }

    public void BuildWheels() {
      var scale = 0.5f * new Vector3(0.75f, 0.1f, 0.75f);

      var wheel = Vehicle.MakePart(PrimitiveType.Cylinder, "wheel", scale, Color.black);
      Vehicle.AddPart(wheel, new Vector3(1, -0.25f, 0));
      wheel.transform.Rotate(new Vector3(90f, 0, 0));

      wheel = Vehicle.MakePart(PrimitiveType.Cylinder, "wheel", scale, Color.black);
      Vehicle.AddPart(wheel, new Vector3(-1, -0.25f, 0));
      wheel.transform.Rotate(new Vector3(90f, 0, 0));
    }
  }
}