namespace CompositePattern.Case2.Unity2 {
  /// <summary>
  /// * The 'Component' interface
  /// </summary>
  public interface IShape {
    double GetVolume();
  }

  [System.Serializable]
  public class IShapeContainer : IUnifiedContainer<IShape> { }
}