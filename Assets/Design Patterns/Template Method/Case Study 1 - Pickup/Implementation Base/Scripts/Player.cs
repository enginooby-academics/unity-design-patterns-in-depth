#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using UnityEngine;

namespace TemplateMethodPattern.Case1.Base {
  public class Player : MonoBehaviour {
    [SerializeField, HideLabel]
    private Stat healthStat = new Stat(StatName.Health, 5);

    [SerializeField, HideLabel]
    private Stat coinStat = new Stat(StatName.Coin, 0);

    [SerializeField, HideLabel]
    private Stat speedStat = new Stat(StatName.Speed, 10);

    public void AddHealth(int amount) {
      healthStat.Add(amount);
    }

    public void AddCoin(int amount) {
      coinStat.Add(amount);
    }

    public void AddSpeed(int amount, float duration) {
      speedStat.Add(amount, duration, this);
    }

    private TransformOperator _mover;

    private void OnEnable() {
      _mover = GetComponent<TransformOperator>();
      speedStat.OnStatChangeEvent += UpdateSpeed;
    }

    private void OnDisable() {
      speedStat.OnStatChangeEvent += UpdateSpeed;
    }

    private void UpdateSpeed() {
      _mover.TranslationalSpeed = speedStat.CurrentValue * new Vector3(1, 0, 1);
    }
  }
}
