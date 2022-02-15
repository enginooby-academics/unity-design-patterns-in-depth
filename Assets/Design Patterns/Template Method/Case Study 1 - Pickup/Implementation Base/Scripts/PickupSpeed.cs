using UnityEngine;

namespace TemplateMethodPattern.Case1.Base {
  public class PickupSpeed : Pickup {
    [SerializeField] private int _value = 5;

    [SerializeField] private float _duration = 3;

    protected override void OnPickedUp(Player player) => player.AddSpeed(_value, _duration);
  }
}