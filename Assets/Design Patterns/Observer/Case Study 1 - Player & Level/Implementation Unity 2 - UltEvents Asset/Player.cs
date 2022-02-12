using UnityEngine;

namespace ObserverPattern.UltEvents {
  public class Player : ObserverPattern.Player {
    [SerializeField] private GameManager gameManager;

    private void OnEnable() {
      // ! dynamic/script event binding
      gameManager.onLevelIncreasedEvent += UpdateHealthOnNewLevel;
    }

    private void OnDisable() {
      gameManager.onLevelIncreasedEvent -= UpdateHealthOnNewLevel;
    }
  }
}