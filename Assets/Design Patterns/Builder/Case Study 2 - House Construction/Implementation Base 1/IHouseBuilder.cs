namespace BuilderPattern.Case2.Base1 {
  /// <summary>
  /// * The 'Builder' contract
  /// </summary>
  public interface IHouseBuilder {
    void BuildBase();
    void BuildRoof();
    void BuildDoor();
    void BuildWindows();
    void BuildChymney();
  }
}
