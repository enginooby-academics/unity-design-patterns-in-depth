using System.Collections.Generic;
using UnityEngine;

namespace TypeObjectPattern.Case1.Base {
  /// <summary>
  ///   The client/driver class.
  /// </summary>
  public class MonsterSpawner : MonoBehaviour {
    [SerializeField] private List<MonsterType> _monsterTypes = new List<MonsterType>();

    private void Start() {
      var healthySlowMonsterType = new MonsterType(100, 8, 2);
      healthySlowMonsterType.MakeInstance(Vector3.zero);
    }
  }
}