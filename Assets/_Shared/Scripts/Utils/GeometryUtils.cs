using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   Extension methods for calculating vectors in space. Useful for procedural stuffs.
/// </summary>
public static class GeometryUtils {
  /// <summary>
  ///   Return a position on the line formed by 2 given points.<br />
  ///   positionFactor indicates offset from start point (e.g. 2/1: divide line into 2 segments then position is 1-segment
  ///   far away from start point). <br />
  ///   [!Notice] Cast to float when passing factor for accurate result, e.g. (float)a/(float)b
  /// </summary>
  // TODO: elaborate on positionFactor param
  public static Vector3 InBetween(Vector3 startPoint, Vector3 endPoint, float positionFactor = 2 / 1) {
    var lineLength = Vector3.Distance(startPoint, endPoint);
    var dir = (endPoint - startPoint).normalized;
    var startPointOffset = dir * lineLength / positionFactor;

    return startPoint + startPointOffset;
  }

  /// <summary>
  ///   Return a list of points on the line formed by 2 given points. All points have same distance.
  /// </summary>
  public static List<Vector3> PositionsInBetween(Vector3 startPoint, Vector3 endPoint, int numOfPos) {
    var points = new List<Vector3>();
    for (var i = 1; i <= numOfPos; i++) {
      var posFactor = (numOfPos + 1) / (float) i;
      points.Add(InBetween(startPoint, endPoint, posFactor));
    }

    return points;
  }
}