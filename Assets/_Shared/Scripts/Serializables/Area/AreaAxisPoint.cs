// * Define decrete area on 3 axes

using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

[Serializable]
[InlineProperty]
public class AreaAxisPoint : AreaPoint {
  [OnValueChanged(nameof(UpdatePoints))] public Vector3Int pointAmount = Vector3Int.one;

  [OnValueChanged(nameof(UpdatePoints))] public Vector3 pointDistance = Vector3.one;

  public override void UpdatePoints() {
    base.UpdatePoints();
    if (!origin.GameObject) {
      Debug.LogWarning("Origin is not set.");
      return;
    }

    var originPos = origin.GameObject.transform.position;
    for (var i = 0; i < pointAmount.x; i++)
    for (var j = 0; j < pointAmount.y; j++)
    for (var k = 0; k < pointAmount.z; k++) {
      var pos = originPos + new Vector3(i * pointDistance.x, j * pointDistance.y, k * pointDistance.z);
      pointPositions.Add(pos);

      if (useTransforms) pointTransforms.Add(CreatePointTransform(pos));
    }
  }
}