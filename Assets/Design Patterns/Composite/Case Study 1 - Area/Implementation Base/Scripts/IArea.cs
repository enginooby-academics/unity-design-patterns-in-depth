using UnityEngine;

namespace CompositePattern.Case1.Base {
  public interface IArea {
    /// <summary>
    ///   Return a random point inside the area.
    /// </summary>
    Vector3 RandomPoint { get; }

    /// <summary>
    ///   Check if the given position is inside the area.
    /// </summary>
    bool Contains(Vector3 pos);

    void DrawGizmos(Color? color = null);
  }
}