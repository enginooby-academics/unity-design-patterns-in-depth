using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

namespace TemplateMethodPattern.Case1.Base {
  public class Player : MonoBehaviour {
    [SerializeField] [HideLabel] private Stat _healthStat = new(StatName.Health, 5);
    [SerializeField] [HideLabel] private Stat _coinStat = new(StatName.Coin);
    [SerializeField] [HideLabel] private Stat _speedStat = new(StatName.Speed, 10);

    private TransformOperator _mover;

    private void Awake() => _mover = GetComponent<TransformOperator>();

    private void OnEnable() => _speedStat.OnStatChangeEvent += UpdateSpeed;

    private void OnDisable() => _speedStat.OnStatChangeEvent -= UpdateSpeed;

    private void UpdateSpeed() => print("UpdateSpeed");

    // FIX
    // private void UpdateSpeed() => _mover.TranslationalSpeed = _speedStat.CurrentValue * new Vector3(1, 0, 1);

    public void AddHealth(int amount) => _healthStat.Add(amount);

    public void AddCoin(int amount) => _coinStat.Add(amount);

    public void AddSpeed(int amount, float duration) => _speedStat.Add(amount, duration, this);
  }
}