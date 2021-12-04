namespace BuilderPattern.Case1.Base {
  // Our 'abstract Builder' class
  // Definition of what methods will be used to actually build the vehicles.
  public interface IVehicleBuilder {
    // Gets the vehicle instance
    Vehicle Vehicle { get; }

    // Contract methods for building the components
    void BuildFrame();
    void BuildEngine();
    void BuildWheels();
    void BuildDoors();
  }
}