using Sirenix.OdinInspector;
using UnityEngine;

namespace MementoPattern.Case1.Base {
  /// <summary>
  /// The 'Originator' class.
  /// </summary>

  public enum ShapeType { Cube, Sphere, Cylinder } // subset of PrimitiveType for this study case
  public class Shape : SerializedMonoBehaviour {
    [SerializeField, OnValueChanged(nameof(UpdateType)), EnumToggleButtons]
    private ShapeType _type;

    [SerializeField, OnValueChanged(nameof(UpdateColor))]
    private Color _color = Color.cyan;

    [SerializeField, OnValueChanged(nameof(UpdateSize)), Range(.5f, 2f)]
    private float _size = 1f;

    private void UpdateType() => Type = _type;
    private void UpdateColor() => Color = _color;
    private void UpdateSize() => Size = _size;

    public ShapeType Type {
      set {
        _type = value;
        if (!_meshFilter) _meshFilter = GetComponent<MeshFilter>();
        _meshFilter.mesh = PrimitiveUtils.GetPrimitiveMesh(_type);
      }
    }

    public Color Color {
      set {
        _color = value;
        if (!_material) _material = GetComponent<MeshRenderer>().material;
        _material.color = _color;
      }
    }

    public float Size {
      set {
        _size = value;
        transform.localScale = Vector3.one * _size;
      }
    }

    private Material _material;
    private MeshFilter _meshFilter;

    private void Awake() {
      UpdateColor();
      UpdateSize();
      UpdateType();
    }

    [Button]
    public void CreateSnapshot(string snapshotName) {
      if (SaveSystem.Instance.CheckSnapshotExist(snapshotName)) {
        Debug.LogError($"Snapshot named {snapshotName} is already existed. Please provide other name.");
        return;
      }
      var snapshot = new ShapeSnapshot(snapshotName, _type, _color, _size);
      SaveSystem.Instance.AddSnapshot(snapshot);
    }

    public void LoadSnapshot(ShapeSnapshot snapshot) {
      Type = snapshot.Type;
      Color = snapshot.Color;
      Size = snapshot.Size;
    }
  }
}
