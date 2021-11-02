using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern4 {
  public class Player : ObserverPattern.Player {
    [SerializeField] GameManager gameManager;

    // ! Ensure to bind event only when the observer is enabled
    private void OnEnable() {
      gameManager.onLevelIncreasedAction += UpdateHealthOnNewLevel;
    }

    private void OnDisable() {
      gameManager.onLevelIncreasedAction -= UpdateHealthOnNewLevel;
    }
  }
}
