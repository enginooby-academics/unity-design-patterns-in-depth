using UnityEngine;
using static UnityEngine.Mathf;

namespace AdapterPattern.Case2 {
  public class Cube : MonoBehaviour, ISurfaceArea {
    [SerializeField] [Range(1f, 5f)] private float _size;

    private void Start() {
    }

    public double GetSurfaceArea() => 6 * Pow(_size, 2);
  }
}