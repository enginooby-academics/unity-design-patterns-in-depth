using Enginooby.Attribute;
using UnityEngine;

namespace ServiceLocatorPattern.Case1.Unity1 {
  public class Shape : MonoBehaviour {
    [Button]
    private void MoveUp() => Locator.Instance.ShapeMover.Move(gameObject, transform.position.OffsetY(1));

    [Button]
    private void MoveDown() => Locator.Instance.ShapeMover.Move(gameObject, transform.position.OffsetY(-1));

    [Button]
    private void ScaleUp() => Locator.Instance.ShapeResizer.Resize(gameObject, transform.localScale.x * 2);

    [Button]
    private void ScaleDown() => Locator.Instance.ShapeResizer.Resize(gameObject, transform.localScale.x / 2);
  }
}