using UnityEngine;

namespace GOConstruction {
  public class Rotating : MonoBehaviour {
    public float Speed = 30;

    private void Update() => transform.Rotate(Vector3.one * Speed * Time.deltaTime);
  }
}