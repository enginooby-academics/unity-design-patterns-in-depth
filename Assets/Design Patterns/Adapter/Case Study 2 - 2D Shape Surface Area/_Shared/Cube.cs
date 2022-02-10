using UnityEngine;
using static UnityEngine.Mathf;

namespace AdapterPattern.Case2 {
  public class Cube : MonoBehaviour, ISurfaceArea {
    [SerializeField, Range(1f, 5f)]
    private float _size;

    public double GetSurfaceArea() => 6 * Pow(_size, 2);

    void Start() {

    }
  }
}