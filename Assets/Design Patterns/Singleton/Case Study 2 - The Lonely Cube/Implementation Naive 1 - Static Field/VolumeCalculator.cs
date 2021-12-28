using Sirenix.OdinInspector;
using UnityEngine;

namespace SingletonPattern.Case2.Naive1 {
  public class VolumeCalculator : MonoBehaviour {
    [Button]
    public void Calculate() {
      float a = Cube.StaticSize;
      float volume = Mathf.Pow(a, 3);
      print(volume);
    }
  }
}
