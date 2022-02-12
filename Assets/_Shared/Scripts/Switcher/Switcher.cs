using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

// FIX: not work in Edit Mode anymore after exit Play Mode
public abstract class Switcher<T> : MonoBehaviour where T : Object {
  [SerializeField] [HideLabel] public AssetCollection<T> collection = new AssetCollection<T>();

  // For Edit Mode
  private void Reset() {
    collection.onCurrentItemChanged.AddListener(Switch);
    Init();
  }

  // For Play Mode
  private void Start() {
    Reset();
  }

  private void Update() {
    collection.ProcessCollectionInput();
  }

  public abstract void Init();
  public abstract void Switch();
}