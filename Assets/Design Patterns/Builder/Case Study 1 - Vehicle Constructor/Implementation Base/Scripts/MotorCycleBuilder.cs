using UnityEngine;

namespace BuilderPattern.Case1.Base {
  public class MotorCycleBuilder : IVehicleBuilder {
    public MotorCycleBuilder() => Vehicle = new Vehicle("MotorCycle");

    public Vehicle Vehicle { get; }

    public void BuildDoors() {
      // We don't need doors for motorcylce.
    }

    public void BuildEngine() {
      var engine = Vehicle.MakePart(PrimitiveType.Cube, "engine", new Vector3(0.75f, 0.5f, 0.75f), Color.gray);
      Vehicle.AddPart(engine, new Vector3(0, -0.25f, 0));
    }

    public void BuildFrame() {
      var frame = Vehicle.MakePart(PrimitiveType.Cube, "frame", new Vector3(2, 0.5f, 0.5f), Color.red);
      Vehicle.AddPart(frame, Vector3.zero);
    }

    public void BuildWheels() {
      var scale = new Vector3(0.75f, 0.1f, 0.75f);

      var wheel = Vehicle.MakePart(PrimitiveType.Cylinder, "wheel", scale, Color.black);
      Vehicle.AddPart(wheel, new Vector3(1, -0.25f, 0));
      wheel.transform.Rotate(new Vector3(90f, 0, 0));

      wheel = Vehicle.MakePart(PrimitiveType.Cylinder, "wheel", scale, Color.black);
      Vehicle.AddPart(wheel, new Vector3(-1, -0.25f, 0));
      wheel.transform.Rotate(new Vector3(90f, 0, 0));
    }
  }
}