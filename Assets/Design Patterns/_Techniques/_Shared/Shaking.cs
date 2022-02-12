using UnityEngine;

namespace GOConstruction {
  public class Shaking : MonoBehaviour {
    public float Strength = 1f;

    private void Start() => transform.ShakePosition(Strength);
  }
}