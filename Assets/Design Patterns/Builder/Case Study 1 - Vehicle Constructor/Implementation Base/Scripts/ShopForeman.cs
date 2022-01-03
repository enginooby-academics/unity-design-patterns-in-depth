namespace BuilderPattern.Case1.Base {
  /// <summary>
  /// * The 'Director' class.
  /// Tells us how to construct the vehicles.
  /// </summary>
  public class ShopForeman {
    public void Construct(IVehicleBuilder vehicleBuilder) {
      vehicleBuilder.BuildFrame();
      vehicleBuilder.BuildEngine();
      vehicleBuilder.BuildWheels();
      vehicleBuilder.BuildDoors();
    }
  }
}
