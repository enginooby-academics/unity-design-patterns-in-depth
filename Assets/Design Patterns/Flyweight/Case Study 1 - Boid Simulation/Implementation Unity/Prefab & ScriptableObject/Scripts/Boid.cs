using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Flyweight.Unity.PrefabAndScriptableObject {
  public class Boid : Flyweight.Boid {
    [SerializeField] private BoidCommonData _boidData;

    protected override void DisplayInfo() {
      base.DisplayInfo();
      InfoPanel.Instance.specieLabel.text = _boidData.Specie.ToString();
      InfoPanel.Instance.endangeredLabel.text = _boidData.EndangeredStatus.ToString();
    }
  }
}