using UnityEngine;
using static SingletonPattern.Case2.Cube;

namespace SingletonPattern.Case2.Unity4 {
  public class Cube : MonoBehaviourSingleton<Cube> {
    protected float _size;
    public float Size => _size;

    void Start() {
      if (_size == 0) _size = Random.Range(1f, 5f);
      Setup(gameObject, _size);
    }
  }
}