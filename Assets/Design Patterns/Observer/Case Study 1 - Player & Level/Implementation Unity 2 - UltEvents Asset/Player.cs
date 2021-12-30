using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern.UltEvents {

  public class Player : ObserverPattern.Player {
    [SerializeField] GameManager gameManager;

    void OnEnable() {
      // ! dynamic/script event binding
      gameManager.onLevelIncreasedEvent += UpdateHealthOnNewLevel;
    }

    private void OnDisable() {
      gameManager.onLevelIncreasedEvent -= UpdateHealthOnNewLevel;
    }
  }
}
