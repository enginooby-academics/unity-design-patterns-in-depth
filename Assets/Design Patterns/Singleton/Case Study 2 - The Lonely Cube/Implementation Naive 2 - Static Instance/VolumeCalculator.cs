using Sirenix.OdinInspector;
using UnityEngine;

namespace SingletonPattern.Case2.Naive2 {
  public class VolumeCalculator : MonoBehaviour {
    [Button]
    public void Calculate() {
      float a = Cube.Instance.Size;
      float volume = Mathf.Pow(a, 3);
      print(volume);
    }
  }
}
