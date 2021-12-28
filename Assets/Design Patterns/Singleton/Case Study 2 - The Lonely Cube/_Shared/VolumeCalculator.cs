using Sirenix.OdinInspector;
using UnityEngine;

namespace SingletonPattern.Case2 {
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
