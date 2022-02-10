using UnityEngine;

namespace GOConstruction {
  public class Shaking : MonoBehaviour {
    public float Strength = 1f;

    void Start() => transform.ShakePosition(Strength);
  }
}