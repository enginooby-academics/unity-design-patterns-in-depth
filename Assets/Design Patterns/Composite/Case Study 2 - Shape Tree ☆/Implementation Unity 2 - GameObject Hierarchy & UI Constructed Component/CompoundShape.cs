using System;
using System.Collections.Generic;
using DG.Tweening;
using Drawing;
using Sirenix.OdinInspector;
using UnityEngine;
using static UnityEngine.Mathf;

namespace CompositePattern.Case2.Unity2 {
  public enum ShapeType { Cube, Sphere }

  /// <summary>
  /// * The 'Composite' class
  /// Optionally extends Leaf base class.
  /// </summary>
  [Serializable, InlineProperty]
  public class CompoundShape : Shape {
    [SerializeField, EnumToggleButtons, OnValueChanged(nameof(UpdateShapeType))]
    private ShapeType _shapeType = ShapeType.Cube;

    [Serializable, InlineProperty]
    class ChildPrefabConfig {
      public IShapeContainer Prefab;

      [Range(0, 5)]
      public int Amount = 1;
    }

    [SerializeField]
    private List<ChildPrefabConfig> _childrenPrefabs = new List<ChildPrefabConfig>();

    private void UpdateShapeType() => gameObject.SetPrimitiveMesh(_shapeType);

    private void CreateNewChild(ChildPrefabConfig prefabConfig) {
      for (int i = 0; i < prefabConfig.Amount; i++) {
        IShape child = Instantiate(prefabConfig.Prefab.Object) as IShape;
        (child as MonoBehaviour).transform.SetParent(transform);
      }
    }

    [Button]
    public void CreateNewChildren() {
      _childrenPrefabs.ForEach(CreateNewChild);
      SetupChildren();
    }

    private void SetupChildren() {
      for (int i = 0; i < transform.childCount; i++) {
        Vector3 newPos = transform.position.OffsetY(-2).OffsetX(2 - 2 * transform.childCount + i * 4);
        transform.GetChild(i).DOMove(newPos, .4f).SetEase(Ease.InOutQuint); ;
      }
    }

    public void DrawLink() {
      using (Draw.ingame.WithLineWidth(2f)) {
        foreach (Transform child in transform) {
          Draw.ingame.Line(transform.position, child.position);
        }
      }
    }

    public override void DrawGizmos() => DrawLink();

    public override double GetVolume() {
      double childrenVolume = 0;
      foreach (Transform child in transform) {
        childrenVolume += child.GetComponent<IShape>().GetVolume();
      }

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
