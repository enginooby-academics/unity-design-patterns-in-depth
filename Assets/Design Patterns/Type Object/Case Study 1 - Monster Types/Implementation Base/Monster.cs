#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using UnityEngine;

namespace TypeObjectPattern.Case1.Base {
  /// <summary>
  /// The 'Typed Object' class, whose type is defined by an instance of MonsterType class.
  /// </summary>
  public class Monster : MonoBehaviour {
    private int _health;
    private MonsterType _type;

    public MonsterType Type {
      get => _type;
      set => _type = value;
    }

    [Button]
    [ContextMenu(nameof(Attack))]
    public void Attack() {
      print($"Monster {name} attacked with {_type.Strength} damages.");
    }

    [Button]
    [ContextMenu(nameof(Move))]
    public void Move() {
      print($"Monster {name} is moving with {_type.Speed} speed.");
    }

    void Start() {
      _health = _type.Health;
    }
  }
}