using UnityEngine;

namespace Flyweight.Unity.PrefabAndScriptableObject {
  [CreateAssetMenu(fileName = "Boid Data", menuName = "Flyweight Pattern/Boid Data", order = 0)]
  public class BoidCommonData : ScriptableObject {
    [SerializeField]
    private Flyweight.Boid.Specie _specie;

    [SerializeField]
    private Flyweight.Boid.EndangeredStatus _endangeredStatus;

    public Flyweight.Boid.Specie Specie => _specie;
    public Flyweight.Boid.EndangeredStatus EndangeredStatus => _endangeredStatus;
  }
}