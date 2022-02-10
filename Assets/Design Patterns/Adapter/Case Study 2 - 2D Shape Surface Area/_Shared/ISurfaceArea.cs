namespace AdapterPattern.Case2 {
  /// <summary>
  /// * [The 'Target' interface]
  /// </summary>
  public interface ISurfaceArea {
    double GetSurfaceArea();
  }

  [System.Serializable]
  public class ISurfaceAreaContainer : IUnifiedContainer<ISurfaceArea> { }
}
