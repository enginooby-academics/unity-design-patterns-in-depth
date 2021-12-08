using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Extension methods for calculating vectors in space. Useful for procedural stuffs.
/// </summary>
public static class GeometryUtils {
  /// <summary>
  /// Return a position on the line formed by 2 given points. 
  /// positionFactor indicate offset from start point (e.g. 2/1: divide line into 2 segments then position is on the 1st).
  /// </summary>
  // TODO: elaborate on positionFactor param
  public static Vector3 InBetween(Vector3 startPoint, Vector3 endPoint, float positionFactor = 2 / 1) {
    // IMPL: different axises, angles
    float lineLength = Vector3.Distance(startPoint, endPoint);
    Vector3 dir = (endPoint - startPoint).normalized;
    Vector3 offsetFromStartPoint = dir * lineLength / positionFactor;

    return startPoint + offsetFromStartPoint;
  }

  /// <summary>
  /// Return a list of points on the line formed by 2 given points. All points have same distance. 
  /// </summary>
  public static List<Vector3> PositionsInBetween(Vector3 startPoint, Vector3 endPoint, int numOfPos) {
    List<Vector3> points = new List<Vector3>();
    for (int i = 1; i <= numOfPos; i++) {
      float pos = (float)(numOfPos + 1) / (float)i;
      points.Add(InBetween(startPoint, endPoint, pos));
    }

    return points;
  }
}