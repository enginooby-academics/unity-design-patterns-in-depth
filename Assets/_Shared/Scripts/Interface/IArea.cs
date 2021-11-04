// * Define area for spawner, detector, boundary, vision...
using UnityEngine;

public interface IArea {
  /// <summary>
  /// Check if the given position is inside the area.
  /// </summary>
  bool Contains(Vector3 pos);
  void DrawGizmos(Color? color = null);

  // ? Vector3 Random => return a random position inside the area
}
