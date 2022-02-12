using UnityEngine;

namespace ObserverPattern.UIEventSystem {
  public class Cube : MonoBehaviour, IColorizable {
    private Renderer _renderer;

    private void Start() {
      _renderer = GetComponent<Renderer>();
    }

    public void UpdateColor(Color newColor) {
      _renderer.material.color = newColor;
    }
  }
}