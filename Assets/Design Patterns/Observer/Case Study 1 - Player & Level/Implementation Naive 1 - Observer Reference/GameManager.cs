using ObserverPattern;
using UnityEngine;

namespace ObserverPattern1 {
  public class GameManager : ObserverPattern.GameManager {
    // ! Observer reference: tight coupling
    [SerializeField] private Player player;

    // ! Unsubscribe to avoid memory leak
    protected override void OnLevelIncreasedCallback() {
      player.UpdateHealthOnNewLevel(_level); // !
    }
  }
}