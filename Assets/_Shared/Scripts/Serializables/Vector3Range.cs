using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

// TODO
// + Randomize

[Serializable]
[InlineProperty]
public class Vector3Range {
  private const float LabelWidth = 40;

  [FoldoutGroup("$title")] [HideInInspector]
  public string title;

  [FoldoutGroup("$title")] [LabelWidth(LabelWidth)] [LabelText("X")]
  public Vector2Wrapper xRange = new();

  [FoldoutGroup("$title")] [LabelWidth(LabelWidth)] [LabelText("Y")]
  public Vector2Wrapper yRange = new();

  [FoldoutGroup("$title")] [LabelWidth(LabelWidth)] [LabelText("Z")]
  public Vector2Wrapper zRange = new();

  [FoldoutGroup("$title")] [LabelWidth(LabelWidth)] [LabelText("All")] [OnValueChanged(nameof(OnAllUpdate), true)]
  public Vector2Wrapper all = new();

  [HideInInspector] public Vector2 initialMinMax;

  public Vector3Range(string title, Vector2? initialMinMax = null) {
    this.title = title;
    var globalMinMax = initialMinMax ?? new Vector2(-10, 10);
    xRange.min = yRange.min = zRange.min = globalMinMax.x;
    xRange.max = yRange.max = zRange.max = globalMinMax.y;
  }

  /// <summary>
  ///   Return a random position lie inside the "box" created by Vector3Range
  /// </summary>
  public Vector3 Random => new(xRange.Random, yRange.Random, zRange.Random);

  // ? Rename to Center
  public Vector3 Average => new(xRange.Average, yRange.Average, zRange.Average);
  public Vector3 Size => new(xRange.Length, yRange.Length, zRange.Length);

  private void OnAllUpdate() {
    // ? define = operator for Vector2Wrapper
    xRange.Value = yRange.Value = zRange.Value = all.Value;
    xRange.min = yRange.min = zRange.min = all.min;
    xRange.max = yRange.max = zRange.max = all.max;
  }

  // TODO: Docs
  public bool ContainsIgnoreZero(Vector3 pos) =>
    true
    && xRange.ContainsIgnoreZero(pos.x)
    && yRange.ContainsIgnoreZero(pos.y)
    && zRange.ContainsIgnoreZero(pos.z);
}