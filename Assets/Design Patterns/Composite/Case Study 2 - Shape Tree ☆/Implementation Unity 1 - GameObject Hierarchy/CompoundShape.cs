#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginooby.Attribute;
#endif

#if ASSET_DOTWEEN
using DG.Tweening;
#endif

#if ASSET_ALINE
using Drawing;
#endif

using System;
using UnityEngine;
using static UnityEngine.Mathf;

namespace CompositePattern.Case2.Unity1 {
  public enum ShapeType {
    Cube,
    Sphere,
  }

  /// <summary>
  ///   * The 'Composite' class
  ///   Optionally extends Leaf base class.
  /// </summary>
  [Serializable]
  [InlineProperty]
  public class CompoundShape : Shape {
    [SerializeField] [EnumToggleButtons] [OnValueChanged(nameof(UpdateShapeType))]
    private ShapeType _shapeType = ShapeType.Cube;

    [SerializeField] private ReferenceConcreteType<IShape> _newChildType;

    protected override void Start() {
      base.Start();
      gameObject.name = "Compound Shape";
      gameObject.SetPrimitiveMesh(_shapeType);
    }


    private void UpdateShapeType() => gameObject.SetPrimitiveMesh(_shapeType);

    [Button]
    public void CreateNewChild() {
      var child = _newChildType.CreateInstance();
      (child as MonoBehaviour).transform.SetParent(transform);
      SetupChildren();
    }

    private void SetupChildren() {
      for (var i = 0; i < transform.childCount; i++) {
        var newPos = transform.position.OffsetY(-2).OffsetX(2 - 2 * transform.childCount + i * 4);
#if ASSET_DOTWEEN
        transform.GetChild(i).DOMove(newPos, .4f).SetEase(Ease.InOutQuint);
        ;
#endif
      }
    }

    public void DrawLink() {
#if ASSET_ALINE
      using (Draw.ingame.WithLineWidth(2f)) {
        foreach (Transform child in transform) Draw.ingame.Line(transform.position, child.position);
      }
#endif
    }

    public override void DrawGizmos() => DrawLink();

    public override double GetVolume() {
      double childrenVolume = 0;
      foreach (Transform child in transform) childrenVolume += child.GetComponent<IShape>().GetVolume();

      double selfVolume = _shapeType switch {
        ShapeType.Cube => Pow(_scale, 3),
        ShapeType.Sphere => 4 / 3 * PI * Pow(_scale / 2, 3),
        _ => throw new ArgumentOutOfRangeException(),
      };

      return childrenVolume + selfVolume;
    }
  }
}