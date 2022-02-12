using UnityEngine;

namespace BuilderPattern.Case1.Base {
  public class BuilderController : MonoBehaviour {
    private void Start() {
      // Instantiate the director and builders
      var shopForeman = new ShopForeman();
      var carBuilder = new CarBuilder();
      var motorCycleBuilder = new MotorCycleBuilder();
      var scooterBuilder = new ScooterBuilder();

      // Make the products, the vehicles.
      shopForeman.Construct(carBuilder);
      shopForeman.Construct(motorCycleBuilder);
      shopForeman.Construct(scooterBuilder);

      // Get the vehicles and access their methods.
      var car = carBuilder.Vehicle;
      Debug.Log(car.GetPartsList());

      var motorCycle = motorCycleBuilder.Vehicle;
      Debug.Log(motorCycle.GetPartsList());

      var scooter = scooterBuilder.Vehicle;
      Debug.Log(scooter.GetPartsList());


      // These calls don't have anything to do with the pattern.
      // They are simply here to make our visual display of the vehicles
      // in the Unity scene look nice.
      car.parent.transform.position = new Vector3(-6f, 0, 0);
      motorCycle.parent.transform.position = new Vector3(6f, 0, 0);
    }
  }
}