using Enginooby.Attribute;
using UnityEngine;

namespace TypeObjectPattern.Case1.Base {
  /// <summary>
  ///   The 'Typed Object' class, whose type is defined by an instance of MonsterType class.
  /// </summary>
  public class Monster : MonoBehaviour {
    private int _currentHealth;

    public MonsterType Type { get; set; }

    private void Start() => _currentHealth = Type.Health;

    [Button]
    [ContextMenu(nameof(Attack))]
    public void Attack() => print($"Monster {name} attacked with {Type.Strength} damages.");

    [Button]
    [ContextMenu(nameof(Move))]
    public void Move() => print($"Monster {name} is moving with {Type.Speed} speed.");
  }
}