using Sirenix.OdinInspector;
using UnityEngine;

namespace Flyweight.Unity.PrefabAndScriptableObject {
  public class Boid : Flyweight.Boid {
    [SerializeField, InlineEditor]
    private BoidCommonData _boidData;

    protected override void DisplayInfo() {
      base.DisplayInfo();
      InfoPanel.Instance.specieLabel.text = _boidData.Specie.ToString();
      InfoPanel.Instance.endangeredLabel.text = _boidData.EndangeredStatus.ToString();
    }
  }
}
