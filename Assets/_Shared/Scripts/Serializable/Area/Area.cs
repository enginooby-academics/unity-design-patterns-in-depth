using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

// ? Create Serializable AreaMultiple/Areas
// ? Rename AreaComposite/AreaSingle
// REFACTOR: Use Composite Pattern
[Serializable, InlineProperty]
public class Area : SerializableBase, IArea {

  protected override void OnComponentOwnerChange() {
    areaPoint.origin.componentOwner = componentOwner;
    areaAxis.componentOwner = componentOwner;
    areaCircular.SetComponentOwner(componentOwner);
    area2DFuncPoint.SetComponentOwner(componentOwner);
  }

  // ? Use flag enum to enable multiple area types at the same time
  public enum AreaType { Axis, Point, Point2dFunc, Circular } // TODO: Polygon type w/ number of vertices: triangle (3), square (4)...

  [OnValueChanged(nameof(UpdateCurrentArea))]
  [HideLabel, EnumToggleButtons] public AreaType areaType = AreaType.Axis;
  [HideInInspector] public IArea currentArea; // ? Remove this since had setter

  public IArea CurrentArea => areaType switch
  {
    AreaType.Axis => areaAxis,
    AreaType.Point => areaPoint,
    AreaType.Point2dFunc => area2DFuncPoint,
    AreaType.Circular => areaCircular,
    _ => null
  };

  private void UpdateCurrentArea() {
    currentArea = areaType switch
    {
      AreaType.Axis => areaAxis,
      AreaType.Point => areaPoint,
      AreaType.Point2dFunc => area2DFuncPoint,
      AreaType.Circular => areaCircular,
      _ => null
    };
  }


  public bool IsAxixType { get => areaType == AreaType.Axis; }
  public bool IsPointType { get => areaType == AreaType.Point || areaType == AreaType.Point2dFunc; }

  [ShowIf(nameof(areaType), AreaType.Axis)]
  [HideLabel] public AreaAxis areaAxis;

  [ShowIf(nameof(areaType), AreaType.Point)]
  [HideLabel] public AreaAxisPoint areaPoint;

  [ShowIf(nameof(areaType), AreaType.Point2dFunc)]
  [HideLabel] public Area2DFuncPoint area2DFuncPoint;

  [ShowIf(nameof(areaType), AreaType.Circular)]
  [HideLabel] public AreaCircular areaCircular = new AreaCircular();

  public Area() {
    currentArea = areaAxis;
  }

  public bool Contains(Vector3 pos) {
    if (currentArea == null) UpdateCurrentArea();
    return currentArea.Contains(pos);
  }

  public void DrawGizmos(Color? color = null) {
    if (currentArea == null) UpdateCurrentArea();
    currentArea.DrawGizmos(color);
  }
}