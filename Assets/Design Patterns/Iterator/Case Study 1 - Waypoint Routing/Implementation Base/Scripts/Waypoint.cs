using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

namespace IteratorPattern.Case1.Base {
  public class Waypoint : MonoBehaviour {
    public enum Color {
      Green,
      Red
    }

    [SerializeField] [OnValueChanged(nameof(UpdateColor))] [EnumToggleButtons]
    private Color _color;

    [SerializeField] [OnValueChanged(nameof(UpdateSize))] [Range(.5f, 2f)]
    private float _size = 1f;

    public float GetSize() => _size;
    public Color GetColor() => _color;

    // IMPL
    private void UpdateColor() {
      var mat = GetComponent<MeshRenderer>().sharedMaterial;
      mat.color = _color == Color.Green ? UnityEngine.Color.green : UnityEngine.Color.red;
    }

    private void UpdateSize() {
      transform.localScale = Vector3.one * _size;
    }
  }
}