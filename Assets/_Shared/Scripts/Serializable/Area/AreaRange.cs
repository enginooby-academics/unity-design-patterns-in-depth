// * Define area from line segment. Use case: attackable/shootable range
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

[Serializable, InlineProperty]
public class AreaRange : SerializableBase, IArea {
  [BoxGroup("$label")]
  [HideLabel] public Reference origin; // ? Replace by ReferenceVector3
  [HideInInspector] public string label;

  [BoxGroup("$label")]
  public float length = 10f;

  public override void SetComponentOwner(GameObject componentOwner) {
    base.SetComponentOwner(componentOwner);
    origin.SetComponentOwner(componentOwner);
    origin.GameObject = componentOwner;
  }

  public AreaRange(string label = "Range", float length = 10f) {
    this.label = label;
    this.length = length;
  }

  public bool Contains(Vector3 pos) {
    // https://learn.unity.com/tutorial/chasing-the-player?uv=2019.4&projectId=5e0b85cdedbc2a144cf5cde5#5e0b8be8edbc2a035d135cd8
    Vector3 directionToPos = pos - origin.GameObject.transform.position;
    return directionToPos.magnitude < length;
  }

  public void DrawGizmos(Color? color = null) {
    if (!origin.GameObject) return;

    color ??= Color.magenta;
    Gizmos.color = color.Value;
  }
}