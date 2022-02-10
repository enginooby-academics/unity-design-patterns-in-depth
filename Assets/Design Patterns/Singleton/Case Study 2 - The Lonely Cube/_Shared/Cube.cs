using System;
using UnityEngine;

namespace SingletonPattern.Case2 {
  public abstract class Cube {
    private float _size;

    public float Size => _size;

    private GameObject _gameObject;
    private int _count;

    protected void Init() {
      _gameObject = new GameObject("Cube");
      if (_size == 0) _size = UnityEngine.Random.Range(1f, 5f);
      MonoBehaviourCube.Setup(_gameObject, _size);

      int delay = 5 - DateTime.Now.Second;
      new System.Threading.Timer(o => Debug.Log(_count++), null, delay * 1000, 3000);
    }
  }
}
