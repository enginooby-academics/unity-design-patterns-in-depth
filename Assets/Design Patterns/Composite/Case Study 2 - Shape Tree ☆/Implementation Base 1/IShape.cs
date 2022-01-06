using UnityEngine;

namespace CompositePattern.Case2.Base1 {
  /// <summary>
  /// * The 'Component' interface
  /// Unnessary if the Composite is also a Leaf
  /// </summary>
  public interface IShape {
    double GetVolume();

    GameObject GameObject { get; set; }
  }
}