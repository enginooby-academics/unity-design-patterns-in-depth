// * Define decrete area using points
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

[Serializable, InlineProperty]
public abstract class AreaPoint : SerializableBase, IArea {
  [OnValueChanged(nameof(UpdatePoints), true)]
  [LabelText("Area Origin")] public Reference origin; // ? Replace by ReferenceVector3

  [OnValueChanged(nameof(UpdatePoints))]
  public float pointRadius = .5f;

  [HideInInspector] public List<Vector3> pointPositions = new List<Vector3>();

  [InfoBox("Create Transforms as children of Origin, useful for manual positioning points or moving Origin.")]
  [OnValueChanged(nameof(UpdatePoints))]
  [InlineButton(nameof(ClearPointTransforms), "Clear")]
  [LabelText("Generate Point Transforms")]
  [ToggleLeft]
  public bool useTransforms = true;

  public List<Transform> pointTransforms = new List<Transform>();

  public void ClearPointTransforms() {
    pointTransforms.ForEach(transform => { if (transform) UnityEngine.Object.DestroyImmediate(transform.gameObject); });
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

    GameObject pointTransform = new GameObject(name);
    // pointTransform.AddComponent<SpawnPoint>(); // ? Replace/remove SpanwPoint component 
    pointTransform.transform.position = pos;
    pointTransform.transform.SetParent(origin.GameObject.transform);
    return pointTransform.transform;
  }

  public bool Contains(Vector3 pos) {
    foreach (Transform point in pointTransforms) {
      if (point.position == pos) return true; // ? consider pos inside point radius
    }

    return false;
  }

  public void DrawGizmos(Color? color = null) {
    if (pointPositions.IsUnset() || pointTransforms.IsUnset()) return;

    color ??= Color.magenta;
    Gizmos.color = color.Value;
    if (useTransforms) {
      pointTransforms.ForEach(transform => {
        if (!transform) pointTransforms.Remove(transform);
        Gizmos.DrawSphere(transform.position, pointRadius);
      });
    } else {
      pointPositions.ForEach(point => Gizmos.DrawSphere(point, pointRadius));
    }
  }
}