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
  }

  // ? Use flag enum to enable multiple area types at the same time
  public enum AreaType { Axis, Point, Circular } // TODO: Polygon type w/ number of vertices: triangle (3), square (4)...

  [OnValueChanged(nameof(UpdateCurrentArea))]
  [HideLabel, EnumToggleButtons] public AreaType areaType = AreaType.Axis;
  [SerializeField] private IArea currentArea;
  private void UpdateCurrentArea() {
    currentArea = areaType switch
    {
      AreaType.Axis => areaAxis,
      AreaType.Point => areaPoint,
      AreaType.Circular => areaCircular,
      _ => null
    };
  }

  public bool IsAxixType { get => areaType == AreaType.Axis; }
  public bool IsPointType { get => areaType == AreaType.Point; }


  [ShowIf(nameof(areaType), AreaType.Axis)]
  [HideLabel] public AreaAxis areaAxis;

  [ShowIf(nameof(areaType), AreaType.Point)]
  [HideLabel] public AreaPoint areaPoint;
  [ShowIf(nameof(areaType), AreaType.Circular)]
  [HideLabel] public AreaCircular areaCircular = new AreaCircular();

  public bool Contains(Vector3 pos) {
    if (currentArea == null) UpdateCurrentArea();
    return currentArea.Contains(pos);
  }

  public void DrawGizmos(Color? color = null) {
    if (currentArea == null) UpdateCurrentArea();
    currentArea.DrawGizmos(color);
  }
}