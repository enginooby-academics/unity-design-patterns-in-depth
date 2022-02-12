using System;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SingletonPattern.Case2 {
  public abstract class Cube {
    private int _count;

    private GameObject _gameObject;

    public float Size { get; private set; }

    protected void Init() {
      _gameObject = new GameObject("Cube");
      if (Size == 0) Size = Random.Range(1f, 5f);
      MonoBehaviourCube.Setup(_gameObject, Size);

      var delay = 5 - DateTime.Now.Second;
      new Timer(o => Debug.Log(_count++), null, delay * 1000, 3000);
    }
  }
}