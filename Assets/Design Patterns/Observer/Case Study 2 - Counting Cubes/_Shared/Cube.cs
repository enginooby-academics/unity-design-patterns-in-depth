using UnityEngine;

namespace ObserverPattern.Case2 {
  public class Cube : MonoBehaviour {
    private void Start() {
      Appear();
      transform.ShakeRotation();
    }

    private void Appear() {
      gameObject.SetMaterialColor(Random.ColorHSV());
      gameObject.transform.position = new Vector3(6, 2.5f, 3).RandomRange();
    }

    private void OnMouseDown() {
      Appear();
      Counter.Instance.Count++;
    }
  }
}
