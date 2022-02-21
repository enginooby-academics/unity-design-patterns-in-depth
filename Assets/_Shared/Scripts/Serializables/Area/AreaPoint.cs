// * Define decrete area using points

using System;
using System.Collections.Generic;
using Enginooby.Utils;
using UnityEngine;
using Object = UnityEngine.Object;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

[Serializable]
[InlineProperty]
public abstract class AreaPoint : SerializableBase, IArea {
  [OnValueChanged(nameof(UpdatePoints), true)] [LabelText("Area Origin")]
  public Reference origin; // ? Replace by ReferenceVector3

  [OnValueChanged(nameof(UpdatePoints))] public float pointRadius = .5f;

  [HideInInspector] public List<Vector3> pointPositions = new();

  [InfoBox("Create Transforms as children of Origin, useful for manual positioning points or moving Origin.")]
  [OnValueChanged(nameof(UpdatePoints))]
  [InlineButton(nameof(ClearPointTransforms), "Clear")]
  [LabelText("Generate Point Transforms")]
  [ToggleLeft]
  public bool useTransforms = true;

  public List<Transform> pointTransforms = new();

  public bool Contains(Vector3 pos) {
    foreach (var point in pointTransforms)
      if (point.position == pos)
        return true; // ? consider pos inside point radius

    return false;
  }

  public void DrawGizmos(Color? color = null) {
    if (pointPositions.IsNullOrEmpty() || pointTransforms.IsNullOrEmpty()) return;

    color ??= Color.magenta;
    Gizmos.color = color.Value;
    if (useTransforms)
      pointTransforms.ForEach(transform => {
        if (!transform) pointTransforms.Remove(transform);
        Gizmos.DrawSphere(transform.position, pointRadius);
      });
    else
      pointPositions.ForEach(point => Gizmos.DrawSphere(point, pointRadius));
  }

  public void ClearPointTransforms() {
    pointTransforms.ForEach(transform => {
      if (transform) Object.DestroyImmediate(transform.gameObject);
    });
    pointTransforms.Clear();
  }

  public virtual void UpdatePoints() {
    if (!origin.GameObject) {
      Debug.LogWarning("Origin is not set.");
      return;
    }

    pointPositions.Clear();
    ClearPointTransforms();
  }

  protected Transform CreatePointTransform(Vector3 pos, string name = "Area Point") {
    if (!origin.GameObject) return null;

    var pointTransform = new GameObject(name);
    pointTransform.transform.position = pos;
    pointTransform.transform.SetParent(origin.GameObject.transform);
    return pointTransform.transform;
  }
}