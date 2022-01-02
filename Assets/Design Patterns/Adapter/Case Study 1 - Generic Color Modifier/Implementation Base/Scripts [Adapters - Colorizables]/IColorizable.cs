using System;
using UnityEngine;

namespace AdapterPattern.Case1.Base1 {
  /// <summary>
  /// * [The 'Target' interface]
  /// </summary>
  public interface IColorizable {
    Color Color { get; set; }
    Type ComponentType { get; }
  }
}
