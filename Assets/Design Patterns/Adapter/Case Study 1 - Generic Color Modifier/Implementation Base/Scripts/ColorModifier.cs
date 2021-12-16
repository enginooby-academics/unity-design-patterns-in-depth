using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Adapter.Base {
  public class ColorModifier : MonoBehaviour {
    private IColorizable _colorizableTarget;

    private void InitColorizableTarget() {
      if (TryGetComponent<Image>(out var image)) {
        _colorizableTarget = new ColorizableImage(image);
      } else if (TryGetComponent<Text>(out var text)) {
        _colorizableTarget = new ColorizableText(text);
      } else if (TryGetComponent<MeshRenderer>(out var meshRenderer)) {
        _colorizableTarget = new ColorizableMaterial(meshRenderer.sharedMaterial);
      }
    }

    [Button]
    public void SetColor(Color color) {
      if (_colorizableTarget == null) InitColorizableTarget();
      _colorizableTarget.Color = color;
    }
  }
}
