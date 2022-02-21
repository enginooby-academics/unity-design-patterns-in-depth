using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

// ? Create Serializable AreaMultiple/Areas
// ? Rename AreaComposite/AreaSingle
// REFACTOR: Use Composite Pattern
[Serializable]
[InlineProperty]
public class Area : SerializableBase, IArea {
  // ? Use flag enum to enable multiple area types at the same time
  public enum AreaType {
    Axis,
    Point,
    Point2dFunc,
    Circular,
  } // TODO: Polygon type w/ number of vertices: triangle (3), square (4)...

  [OnValueChanged(nameof(UpdateCurrentArea))] [HideLabel] [EnumToggleButtons]
  public AreaType areaType = AreaType.Axis;

  [ShowIf(nameof(areaType), AreaType.Axis)] [HideLabel]
  public AreaAxis areaAxis;

  [ShowIf(nameof(areaType), AreaType.Point)] [HideLabel]
  public AreaAxisPoint areaPoint;

  [ShowIf(nameof(areaType), AreaType.Point2dFunc)] [HideLabel]
  public Area2DFuncPoint area2DFuncPoint;

  [ShowIf(nameof(areaType), AreaType.Circular)] [HideLabel]
  public AreaCircular areaCircular = new();

  [HideInInspector] public IArea currentArea; // ? Remove this since had setter

  public Area() => currentArea = areaAxis;

  public IArea CurrentArea =>
    areaType switch {
      AreaType.Axis => areaAxis,
      AreaType.Point => areaPoint,
      AreaType.Point2dFunc => area2DFuncPoint,
      AreaType.Circular => areaCircular,
      _ => null,
    };

  public bool IsAxixType => areaType == AreaType.Axis;
  public bool IsPointType => areaType == AreaType.Point || areaType == AreaType.Point2dFunc;

  public bool Contains(Vector3 pos) {
    if (currentArea == null) UpdateCurrentArea();
    return currentArea.Contains(pos);
  }

  public void DrawGizmos(Color? color = null) {
    if (currentArea == null) UpdateCurrentArea();
    currentArea.DrawGizmos(color);
  }

  protected override void OnGameObjectChanged() {
    areaPoint.origin.GameObject = GameObject;
    areaAxis.GameObject = GameObject;
    areaCircular.SetGameObject(GameObject);
    area2DFuncPoint.SetGameObject(GameObject);
  }

  private void UpdateCurrentArea() {
    currentArea = areaType switch {
      AreaType.Axis => areaAxis,
      AreaType.Point => areaPoint,
      AreaType.Point2dFunc => area2DFuncPoint,
      AreaType.Circular => areaCircular,
      _ => null,
    };
  }
}