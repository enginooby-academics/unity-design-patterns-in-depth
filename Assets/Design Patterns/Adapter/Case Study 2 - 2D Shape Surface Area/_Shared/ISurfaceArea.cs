using System;

namespace AdapterPattern.Case2 {
  /// <summary>
  ///   * [The 'Target' interface]
  /// </summary>
  public interface ISurfaceArea {
    double GetSurfaceArea();
  }

  [Serializable]
  public class ISurfaceAreaContainer : IUnifiedContainer<ISurfaceArea> {
  }
}