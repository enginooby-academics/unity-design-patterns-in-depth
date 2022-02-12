using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

namespace TypeObjectPattern.Case1.Base {
  /// <summary>
  ///   The 'Type Object' class. Contains all data to define a monster type.
  /// </summary>
  [Serializable]
  [InlineProperty]
  public class MonsterType {
    [SerializeField] private int _health;

    [SerializeField] private int _strength;

    [SerializeField] private int _speed;

    public MonsterType(int health, int strength, int speed) {
      _health = health;
      _strength = strength;
      _speed = speed;
    }

    public int Health => _health;
    public int Strength => _strength;
    public int Speed => _speed;

    [Button]
    public Monster MakeInstance(Vector3 pos, string name = "Monster") {
      var monsterGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
      monsterGo.name = name;
      monsterGo.transform.position = pos;
      var monster = monsterGo.AddComponent<Monster>();
      monster.Type = this;

      return monster;
    }
  }
}