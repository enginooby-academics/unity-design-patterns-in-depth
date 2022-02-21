using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Decorator.Base {
  public class Gun : MonoBehaviour {
    [SerializeField] [InlineEditor(InlineEditorModes.FullEditor)]
    private BulletDriver _bulletPrefab;

    private void Update() {
      if (MouseButton.Left.IsDown()) Shoot();
    }

    public void Shoot() {
      Instantiate(_bulletPrefab);
    }
  }
}