using System.Collections.Generic;
using UnityEngine;

namespace TypeObjectPattern.Case1.Base {
  /// <summary>
  /// The client/driver class.
  /// </summary>
  public class MonsterSpawner : MonoBehaviour {
    [SerializeField]
    private List<MonsterType> _monsterTypes = new List<MonsterType>();

    void Start() {
      var healthySlowMonsterType = new MonsterType(health: 100, strength: 8, speed: 2);
      healthySlowMonsterType.MakeInstance(Vector3.zero);
    }
  }
}
