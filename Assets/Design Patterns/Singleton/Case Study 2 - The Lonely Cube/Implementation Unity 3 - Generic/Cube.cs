using UnityEngine;
using static SingletonPattern.Case2.MonoBehaviourCube;

namespace SingletonPattern.Case2.Unity3 {
  public class Cube : MonoBehaviourSingleton<Cube> {
    public float Size { get; protected set; }

    void Start() {
      if (Size == 0) Size = Random.Range(1f, 5f);
      Setup(gameObject, Size);
    }
  }
}