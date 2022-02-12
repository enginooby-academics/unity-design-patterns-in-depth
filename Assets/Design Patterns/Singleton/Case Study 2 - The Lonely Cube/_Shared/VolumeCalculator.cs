using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

namespace SingletonPattern.Case2 {
  /// <summary>
  ///   To demonstrate how to access the singleton's field.
  /// </summary>
  public abstract class VolumeCalculator : MonoBehaviour {
    [Button]
    public void Calculate() {
      var a = GetCubeSize();
      var volume = Mathf.Pow(a, 3);
      print(volume);
    }

    protected abstract float GetCubeSize();
  }
}