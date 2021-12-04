namespace BuilderPattern.Case1.Base {
  // Our 'Director' class. Tells us how to construct the vehicles.
  public class ShopForeman {
    public void Construct(IVehicleBuilder vehicleBuilder) {
      vehicleBuilder.BuildFrame();
      vehicleBuilder.BuildEngine();
      vehicleBuilder.BuildWheels();
      vehicleBuilder.BuildDoors();
    }
  }
}
