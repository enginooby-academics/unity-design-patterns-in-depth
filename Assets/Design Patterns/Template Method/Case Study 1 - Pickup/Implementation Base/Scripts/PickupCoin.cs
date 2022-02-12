using UnityEngine;

namespace TemplateMethodPattern.Case1.Base {
  public class PickupCoin : Pickup {
    [SerializeField] private int _value = 5;

    protected override void OnPickedUp(Player player) {
      player.AddCoin(_value);
    }
  }
}