using UnityEngine;
using Sirenix.OdinInspector;

namespace Decorator.Base {
  public class Gun : MonoBehaviour {
    [SerializeField, InlineEditor(InlineEditorModes.FullEditor)]
    private BulletDriver _bulletPrefab;

    public void Shoot() {
      Instantiate(_bulletPrefab);
    }

    void Update() {
      if (MouseButton.Left.IsDown()) Shoot();
    }
  }
}
