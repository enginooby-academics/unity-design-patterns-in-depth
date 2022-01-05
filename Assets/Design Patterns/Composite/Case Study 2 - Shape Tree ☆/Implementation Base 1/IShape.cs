using UnityEngine;

namespace CompositePattern.Case2.Base1 {
  /// <summary>
  /// * The 'Component' interface
  /// </summary>
  public interface IShape {
    double GetVolume();

    GameObject GameObject { get; set; }
  }
}