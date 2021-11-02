// * Define area for spawner, detector, boundary...
using UnityEngine;

public interface IArea {
  /// <summary>
  /// Check if the given position is inside the area.
  /// </summary>
  bool Contains(Vector3 pos);
  void DrawGizmos();

  // ? Vector3 Random => return a random position inside the area
}
