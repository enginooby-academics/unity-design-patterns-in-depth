// * Define area from radius & angle. Use case: vision area (where a character can see)
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

// ? Rename to AreaArc3D or AreaSphericalSector
[Serializable, InlineProperty]
public class AreaCircular : SerializableBase, IArea {
  [BoxGroup("$label")]
  [HideLabel] public Reference origin; // ? Replace by ReferenceVector3
  [HideInInspector] public string label;

  [BoxGroup("$label")]
  public float radius = 10f;

  [BoxGroup("$label")]
  public float angle = 360f;

  [BoxGroup("$label")]
  public bool gizmosWire;

  public override void SetComponentOwner(GameObject componentOwner) {
    base.SetComponentOwner(componentOwner);
    origin.SetComponentOwner(componentOwner);
    origin.GameObject = componentOwner;
  }

  public AreaCircular(string label = "Circula Area", float radius = 10f, float angle = 360f) {
    this.label = label;
    this.radius = radius;
    this.angle = angle;
  }

  public bool Contains(Vector3 pos) {
    // https://learn.unity.com/tutorial/chasing-the-player?uv=2019.4&projectId=5e0b85cdedbc2a144cf5cde5#5e0b8be8edbc2a035d135cd8
    Vector3 directionToPos = pos - origin.GameObject.transform.position;
    float angleToPos = Vector3.Angle(directionToPos, origin.GameObject.transform.forward);
    return directionToPos.magnitude < radius && angleToPos < angle;
  }

  // TODO: Declare overloading Contains in IArea
  public bool Contains(GameObject target) {
    return Contains(target.transform.position);
  }

  public bool Contains(Reference reference) {
    return Contains(reference.GameObject);
  }

  public void DrawGizmos(Color? color = null) {
    if (!origin.GameObject) return;

    color ??= Color.magenta;
    Gizmos.color = color.Value;

    // TODO: draw portion of sphere based on angle
    if (gizmosWire) Gizmos.DrawWireSphere(origin.GameObject.transform.position, radius);
    else Gizmos.DrawSphere(origin.GameObject.transform.position, radius);

    // Handles.color = color.Value;
    // Transform transform = origin.GameObject.transform;
    // Handles.DrawSolidArc(transform.position, transform.up, transform.position, angle, radius);
  }
}