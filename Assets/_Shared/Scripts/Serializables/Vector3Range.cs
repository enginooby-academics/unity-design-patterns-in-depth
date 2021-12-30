using UnityEngine;
using Sirenix.OdinInspector;
using System;

// TODO
// + Randomize

[Serializable]
[InlineProperty]
public class Vector3Range {
  private const float LABEL_WIDTH = 40;

  [FoldoutGroup("$title")]
  [HideInInspector] public string title;

  [FoldoutGroup("$title")]
  [LabelWidth(LABEL_WIDTH)]
  [LabelText("X")]
  public Vector2Wrapper xRange = new Vector2Wrapper();

  [FoldoutGroup("$title")]
  [LabelWidth(LABEL_WIDTH)]
  [LabelText("Y")]
  public Vector2Wrapper yRange = new Vector2Wrapper();

  [FoldoutGroup("$title")]
  [LabelWidth(LABEL_WIDTH)]
  [LabelText("Z")]
  public Vector2Wrapper zRange = new Vector2Wrapper();

  [FoldoutGroup("$title")]
  [LabelWidth(LABEL_WIDTH)]
  [LabelText("All")]
  [OnValueChanged(nameof(OnAllUpdate), true)]
  public Vector2Wrapper all = new Vector2Wrapper();

  [HideInInspector] public Vector2 initialMinMax;

  private void OnAllUpdate() {
    // ? define = operatot for Vector2Wrapper
    xRange.Value = yRange.Value = zRange.Value = all.Value;
    xRange.min = yRange.min = zRange.min = all.min;
    xRange.max = yRange.max = zRange.max = all.max;
  }

  public Vector3Range(string title, Vector2? initialMinMax = null) {
    this.title = title;
    Vector2 globalMinMax = initialMinMax ?? new Vector2(-10, 10);
    xRange.min = yRange.min = zRange.min = globalMinMax.x;
    xRange.max = yRange.max = zRange.max = globalMinMax.y;
  }

  /// <summary>
  /// Return a random position lie inside the "box" created by Vector3Range
  /// </summary>
  public Vector3 Random {
    get => new Vector3(xRange.Random, yRange.Random, zRange.Random);
  }

  // ? Rename to Center
  public Vector3 Average { get => new Vector3(xRange.Average, yRange.Average, zRange.Average); }
  public Vector3 Size { get => new Vector3(xRange.Length, yRange.Length, zRange.Length); }

  // TODO: Docs
  public bool ContainsIgnoreZero(Vector3 pos) {
    return true
      && xRange.ContainsIgnoreZero(pos.x)
      && yRange.ContainsIgnoreZero(pos.y)
      && zRange.ContainsIgnoreZero(pos.z)
      ;
  }
}