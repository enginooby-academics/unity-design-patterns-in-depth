#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using UnityEngine;

namespace SingletonPattern.Case2 {
  /// <summary>
  /// To demonstrate how to access the singleton's field.
  /// </summary>
  public abstract class VolumeCalculator : MonoBehaviour {
    [Button]
    public void Calculate() {
      float a = GetCubeSize();
      float volume = Mathf.Pow(a, 3);
      print(volume);
    }

    protected abstract float GetCubeSize();
  }
}
