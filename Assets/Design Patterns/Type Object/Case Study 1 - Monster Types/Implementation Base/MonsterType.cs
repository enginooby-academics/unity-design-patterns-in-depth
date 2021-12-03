using Sirenix.OdinInspector;
using UnityEngine;

namespace TypeObjectPattern.Case1.Base {
  /// <summary>
  /// The 'Type Object' class. Contains all data to define a monster type.
  /// </summary>
  [System.Serializable, InlineProperty]
  public class MonsterType {
    [SerializeField]
    private int _health;

    [SerializeField]
    private int _strength;

    [SerializeField]
    private int _speed;

    public int Health => _health;
    public int Strength => _strength;
    public int Speed => _speed;

    public MonsterType(int health, int strength, int speed) {
      _health = health;
      _strength = strength;
      _speed = speed;
    }

    [Button]
    public Monster MakeInstance(Vector3 pos, string name = "Monster") {
      GameObject monsterGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
      monsterGameObject.name = name;
      monsterGameObject.transform.position = pos;
      Monster monster = monsterGameObject.AddComponent<Monster>();
      monster.Type = this;
      return monster;
    }
  }
}
