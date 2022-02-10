namespace AdapterPattern.Case2 {
  /// <summary>
  /// * [The 'Adaptee' interface]
  /// </summary>
  public interface IArea {
    double GetArea();
  }

  [System.Serializable]
  public class IAreaContainer : IUnifiedContainer<IArea> { }
}
