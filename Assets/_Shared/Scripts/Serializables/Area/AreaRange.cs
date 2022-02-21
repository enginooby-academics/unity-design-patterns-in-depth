// * Define area from line segment. Use case: attackable/shootable range

using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

[Serializable]
[InlineProperty]
public class AreaRange : SerializableBase, IArea {
  [BoxGroup("$label")] [HideLabel] public Reference origin; // ? Replace by ReferenceVector3

  [HideInInspector] public string label;

  [BoxGroup("$label")] public float length = 10f;

  public AreaRange(string label = "Range", float length = 10f) {
    this.label = label;
    this.length = length;
  }

  public bool Contains(Vector3 pos) {
    // https://learn.unity.com/tutorial/chasing-the-player?uv=2019.4&projectId=5e0b85cdedbc2a144cf5cde5#5e0b8be8edbc2a035d135cd8
    var directionToPos = pos - origin.GameObject.transform.position;
    return directionToPos.magnitude < length;
  }

  public void DrawGizmos(Color? color = null) {
    if (!origin.GameObject) return;

    color ??= Color.magenta;
    Gizmos.color = color.Value;
  }

  public override void SetGameObject(GameObject componentOwner) {
    base.SetGameObject(componentOwner);
    origin.SetGameObject(componentOwner);
    origin.GameObject = componentOwner;
  }
}