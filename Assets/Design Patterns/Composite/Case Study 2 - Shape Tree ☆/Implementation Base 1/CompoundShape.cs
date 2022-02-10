using System;
using System.Collections.Generic;
using System.Linq;
using Drawing;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.Mathf;


namespace CompositePattern.Case2.Base1 {
  public enum ShapeType { Cube, Sphere }

  /// <summary>
  /// * The 'Composite' class
  /// Optionally extends Leaf base class.
  /// </summary>
  [Serializable, InlineProperty]
  // ? Rename to CompositeShape
  public class CompoundShape : Shape {
    [SerializeField, EnumToggleButtons, OnValueChanged(nameof(UpdateShapeType))]
    private ShapeType _shapeType = ShapeType.Cube;

    [SerializeReference, OnCollectionChanged(nameof(SetupChildren))]
    private List<IShape> children = new List<IShape>();

    public CompoundShape() : base(PrimitiveType.Cube) { }

    private void UpdateShapeType() => GameObject.SetPrimitiveMesh(_shapeType);

    private void SetupChildren() {
      Vector3 pos = GameObject.transform.position;

      for (int i = 0; i < children.Count; i++) {
        Vector3 newPos = pos.OffsetY(-2).OffsetX(2 - 2 * children.Count + i * 4);
        children[i].GameObject.transform.DOMove(newPos, .4f).SetEase(Ease.InOutQuint); ;
        children[i].GameObject.transform.SetParent(GameObject.transform);
      }
    }

    public void DrawLink() {
      using (Draw.ingame.WithLineWidth(2f)) {
        foreach (var child in children) {
          Draw.ingame.Line(GameObject.transform.position, child.GameObject.transform.position);
          if (child.GetType() == typeof(CompoundShape)) (child as CompoundShape).DrawLink();
        }
      }
    }

    public override double GetVolume() {
      double childrenVolume = children.Sum(child => child.GetVolume());
      double selfVolume = _shapeType switch
      {
        ShapeType.Cube => Pow(_scale, 3),
        ShapeType.Sphere => 4 / 3 * PI * Pow(_scale / 2, 3),
        _ => throw new System.ArgumentOutOfRangeException(),
      };

      return childrenVolume + selfVolume;
    }
  }
}
