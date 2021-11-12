// * Define decrete area using function of 2 variables
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Text;

[Serializable, InlineProperty]
public class Area2DFuncPoint : AreaPoint {
  [OnValueChanged(nameof(UpdatePoints))]
  [Min(1)]
  public int pointAmount = 10;

  [OnValueChanged(nameof(UpdatePoints))]
  [Min(0)]
  public float pointStep = 1;

  [OnValueChanged(nameof(UpdatePoints))]
  [ShowInInspector, DisplayAsString]
  private String formula;
  private StringBuilder formulaBuilder = new StringBuilder();

  [OnValueChanged(nameof(UpdatePoints))]
  public float degree0;
  [OnValueChanged(nameof(UpdatePoints))]
  public float degree1;
  [OnValueChanged(nameof(UpdatePoints))]
  public float degree2;

  protected override void UpdatePoints() {
    base.UpdatePoints();
    if (!origin.GameObject) { // TODO: implement this in base
      Debug.LogWarning("Origin is not set.");
      return;
    }

    UpdateFormula();
    Vector3 originPos = origin.GameObject.transform.position;
    for (int i = 0; i < pointAmount; i++) {
      Vector3 pos = Vector3.zero;
      float k = i * pointStep;
      pos.x = k;
      pos.y = Mathf.Pow(k, 2) * degree2 + Mathf.Pow(k, 1) * degree1 + Mathf.Pow(k, 0) * degree0;
      pos += originPos;

      pointPositions.Add(pos);
      if (useTransforms) {
        pointTransforms.Add(CreatePointTransform(pos));
      }
    }
  }

  private void UpdateFormula() {
    formulaBuilder.Clear();
    string coef2 = (degree2 == 1) ? "x^2 + " : (degree2 == 0) ? "" : degree2 + "x^2 + ";
    string coef1 = (degree1 == 1) ? "x + " : (degree1 == 0) ? "" : degree1 + "x + ";
    string coef0 = (degree0 == 0) ? "" : degree0.ToString();
    formulaBuilder.Append($"y = {coef2} {coef1} {coef0}");
    formula = formulaBuilder.ToString();
  }
}