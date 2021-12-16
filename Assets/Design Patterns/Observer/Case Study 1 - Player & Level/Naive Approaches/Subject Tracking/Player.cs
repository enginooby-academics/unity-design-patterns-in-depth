using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern2 {
  public class Player : ObserverPattern.Player {
    // ! 1: keep subject's reference
    [SerializeField] private ObserverPattern.GameManager gameManager;

    // ! 2: cache subject's state
    private int currentLevel;

    private void Start() {
      currentLevel = gameManager.Level;
    }

    // ! 3: validate caching continously: high cpu usage
    private void TrackGameLevel() {
      if (currentLevel != gameManager.Level) {
        currentLevel = gameManager.Level;
        UpdateHealthOnNewLevel(currentLevel);
      }
    }

    void Update() {
      TrackGameLevel();
    }
  }
}
