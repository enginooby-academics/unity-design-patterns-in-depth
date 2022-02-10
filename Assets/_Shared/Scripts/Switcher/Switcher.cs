#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using UnityEngine;

// FIX: not work in Edit Mode anymore after exit Play Mode
public abstract class Switcher<T> : MonoBehaviour where T : UnityEngine.Object {
  [SerializeField, HideLabel]
  public AssetCollection<T> collection = new AssetCollection<T>();

  private void Update() {
    collection.ProcessCollectionInput();
  }

  // For Edit Mode
  private void Reset() {
    collection.onCurrentItemChanged.AddListener(Switch);
    Init();
  }

  // For Play Mode
  private void Start() {
    Reset();
  }

  public abstract void Init();
  public abstract void Switch();
}
