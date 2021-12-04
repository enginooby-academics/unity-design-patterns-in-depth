using Sirenix.OdinInspector;
using UnityEngine;

namespace MementoPattern.Case1.Base {
  /// <summary>
  /// The 'Originator' class.
  /// </summary>

  public enum ShapeType { Cube, Sphere, Cylinder } // subset of PrimitiveType for this study case

  [TypeInfoBox("Modify properties in Play Mode only!")]
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
        gameObject.SetPrimitiveMesh(_type);
      }
    }

    public Color Color {
      set {
        _color = value;
        gameObject.SetMaterialColor(_color);
      }
    }

    public float Size {
      set {
        _size = value;
        gameObject.SetScale(_size);
      }
    }

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
