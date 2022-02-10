using UnityEngine;
using static UnityEngine.Mathf;

namespace AdapterPattern.Case2 {
  /// <summary>
  /// * [An 'Adaptee' class]
  /// </summary>
  public class Circle : MonoBehaviour, IArea {
    [SerializeField, Range(1f, 5f)]
    private float _radius;

    public double GetArea() => PI * Pow(_radius, 2);

    void Start() {

    }
  }
}