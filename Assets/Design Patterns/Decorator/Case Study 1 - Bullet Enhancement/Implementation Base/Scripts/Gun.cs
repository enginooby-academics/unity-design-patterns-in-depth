#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using UnityEngine;

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
