using Sirenix.OdinInspector;
using UnityEngine;

namespace MementoPattern.Case1.Base {
  /// <summary>
  /// The 'Memento' class. 
  /// Contains all saved data at a certain time as POJO (optionally w/ snapshot metadata).
  /// </summary>
  [System.Serializable, InlineProperty]
  public class ShapeSnapshot {
    [HorizontalGroup("Metadata")]
    [SerializeField, DisplayAsString]
    private string _name; // metadata

    [HorizontalGroup("Metadata")]
    [SerializeField, DisplayAsString]
    private string _timeCreated; // metadata

    [SerializeField, EnumToggleButtons]
    private ShapeType _type;

    [ShowInInspector]
    private Color _color;

    [SerializeField, Range(.5f, 2f)]
    private float _size;

    public string Name => _name;
    public string TimeCreated => _timeCreated;
    public ShapeType Type => _type;
    public Color Color => _color;
    public float Size => _size;

    public ShapeSnapshot(string name, ShapeType type, Color color, float size) {
      _name = name;
      _timeCreated = System.DateTime.Now.ToShortTimeString();
      _type = type;
      _color = color;
      _size = size;
    }

    [Button]
    public void Load() {
      SaveSystem.Instance.LoadSnapshot(this);
    }
  }
}
