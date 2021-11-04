// * Define area from points on 3 axes for spawner, detector, boundary...
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

// ? rename to AreaAxisPoint
[Serializable, InlineProperty]
public class AreaPoint : SerializableBase, IArea {
  [OnValueChanged(nameof(UpdatePoints), true)]
  [LabelText("Area Origin")] public Reference origin; // ? Replace by ReferenceVector3

  [OnValueChanged(nameof(UpdatePoints))]
  public Vector3Int pointAmount = Vector3Int.one;

  [OnValueChanged(nameof(UpdatePoints))]
  public Vector3 pointDistance = Vector3.one;

  [OnValueChanged(nameof(UpdatePoints))]
  public float pointRadius = .5f;

  [HideInInspector] public List<Vector3> pointPositions = new List<Vector3>();

  [InfoBox("Create Transforms as children of Origin, useful for manual positioning points or moving Origin.")]
  [OnValueChanged(nameof(UpdatePoints))]
  public bool useTransforms = true;

  [HideInInspector] public List<Transform> pointTransforms = new List<Transform>();

  [Button]
  public void ClearPointTransforms() {
    pointTransforms.ForEach(transform => { if (transform) UnityEngine.Object.DestroyImmediate(transform.gameObject); });
    pointTransforms.Clear();
  }

  private void UpdatePoints() {
    if (!origin.GameObject) {
      Debug.LogWarning("Origin is not set.");
      return;
    }

    pointPositions.Clear();
    ClearPointTransforms();
    Vector3 originPos = origin.GameObject.transform.position;
    for (int i = 0; i < pointAmount.x; i++) {
      for (int j = 0; j < pointAmount.y; j++) {
        for (int k = 0; k < pointAmount.z; k++) {
          Vector3 pos = originPos + new Vector3(i * pointDistance.x, j * pointDistance.y, k * pointDistance.z);
          pointPositions.Add(pos);

          if (useTransforms) {
            pointTransforms.Add(CreatePointTransform(pos));
          }
        }
      }
    }
  }

  private void UpdatePointPositions() {

  }

  private void UpdatePointTransforms() {
  }

  private Transform CreatePointTransform(Vector3 pos, string name = "Area Point") {
    if (!origin.GameObject) return null;

    GameObject pointTransform = new GameObject(name);
    pointTransform.AddComponent<SpawnPoint>(); // ? Replace/remove SpanwPoint component 
    pointTransform.transform.position = pos;
    pointTransform.transform.SetParent(origin.GameObject.transform);
    return pointTransform.transform;
  }

  public bool Contains(Vector3 pos) {
    // IMPL
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