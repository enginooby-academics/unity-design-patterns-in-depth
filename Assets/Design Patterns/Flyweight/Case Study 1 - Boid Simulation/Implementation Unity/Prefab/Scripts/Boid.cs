using UnityEngine;

namespace Flyweight.Unity.Prefab {
  public class Boid : Flyweight.Boid {
    [SerializeField] private Specie _specie;

    [SerializeField] private EndangeredStatus _endangeredStatus;


    protected override void DisplayInfo() {
      base.DisplayInfo();
      InfoPanel.Instance.specieLabel.text = _specie.ToString();
      InfoPanel.Instance.endangeredLabel.text = _endangeredStatus.ToString();
    }
  }
}