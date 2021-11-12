// * Define decrete area on 3 axes
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

[Serializable, InlineProperty]
public class AreaAxisPoint : AreaPoint {
  [OnValueChanged(nameof(UpdatePoints))]
  public Vector3Int pointAmount = Vector3Int.one;

  [OnValueChanged(nameof(UpdatePoints))]
  public Vector3 pointDistance = Vector3.one;

  protected override void UpdatePoints() {
    base.UpdatePoints();
    if (!origin.GameObject) {
      Debug.LogWarning("Origin is not set.");
      return;
    }

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
}