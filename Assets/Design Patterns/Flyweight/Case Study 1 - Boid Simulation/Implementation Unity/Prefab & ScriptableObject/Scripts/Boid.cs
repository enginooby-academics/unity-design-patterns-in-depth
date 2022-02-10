#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

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
