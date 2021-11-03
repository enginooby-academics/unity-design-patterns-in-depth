using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern1 {
  public class GameManager : ObserverPattern.GameManager {
    // ! Observer reference: tight coupling
    [SerializeField] ObserverPattern.Player player;

    // ! Unsubscribe to avoid memory leak
    protected override void OnLevelIncreasedCallback() {
      player.UpdateHealthOnNewLevel(_level); // !
    }
  }
}