using UnityEngine;
using static UnityEngine.Mathf;

namespace AdapterPattern.Case2 {
  /// <summary>
  /// * [An 'Adaptee' class]
  /// </summary>
  public class Square : MonoBehaviour, IArea {
    [SerializeField, Range(1f, 5f)]
    private float _size;

    public double GetArea() => Pow(_size, 2);

    void Start() {

    }
  }
}