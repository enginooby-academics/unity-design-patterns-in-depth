using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

// ? Create Serializable AreaMultiple/Areas
[Serializable, InlineProperty]
public class Area : SerializableBase, IArea {
  protected override void OnComponentOwnerChange() {
    this.areaPoint.origin.componentOwner = componentOwner;
    this.areaAxis.componentOwner = componentOwner;
  }

  // ? Use flag enum to enable multiple area types at the same time
  public enum AreaType { Axis, Point, Polygon } // Polygon w/ number of vertices: triangle (3), square (4)...
  [HideLabel, EnumToggleButtons] public AreaType areaType = AreaType.Axis;

  public bool IsAxixType { get => areaType == AreaType.Axis; }
  public bool IsPointType { get => areaType == AreaType.Point; }
  public bool IsPolygonType { get => areaType == AreaType.Polygon; }

  [ShowIf(nameof(areaType), AreaType.Axis)]
  [HideLabel] public AreaAxis areaAxis;

  [ShowIf(nameof(areaType), AreaType.Point)]
  [HideLabel] public AreaPoint areaPoint;

  public bool Contains(Vector3 pos) {
    bool contains = false;
    switch (areaType) {
      case AreaType.Axis:
        contains = areaAxis.Contains(pos);
        break;
      case AreaType.Point:
        contains = areaPoint.Contains(pos);
        break;
      case AreaType.Polygon:
        break;
      default:
        break;
    }

    return contains;
  }

  public void DrawGizmos(Color? color = null) {
    switch (areaType) {
      case AreaType.Axis:
        areaAxis.DrawGizmos(color: color);
        break;
      case AreaType.Point:
        areaPoint.DrawGizmos(color: color);
        break;
      case AreaType.Polygon:
        break;
      default:
        break;
    }
  }
}