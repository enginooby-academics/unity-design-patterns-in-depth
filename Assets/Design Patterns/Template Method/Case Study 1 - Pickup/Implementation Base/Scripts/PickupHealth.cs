using UnityEngine;

namespace TemplateMethodPattern.Case1.Base {
  public class PickupHealth : Pickup {
    [SerializeField] private int _value = 1;

    protected override void OnPickedUp(Player player) => player.AddHealth(_value);
  }
}