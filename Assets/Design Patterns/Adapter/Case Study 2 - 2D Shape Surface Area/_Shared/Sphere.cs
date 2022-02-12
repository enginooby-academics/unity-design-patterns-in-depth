using UnityEngine;
using static UnityEngine.Mathf;

namespace AdapterPattern.Case2 {
  public class Sphere : MonoBehaviour, ISurfaceArea {
    [SerializeField] [Range(1f, 5f)] private float _radius;

    private void Start() {
    }

    public double GetSurfaceArea() => 4 * PI * Pow(_radius, 2);
  }
}