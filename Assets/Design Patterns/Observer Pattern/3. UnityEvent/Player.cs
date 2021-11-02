using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern3 {
  public class Player : ObserverPattern.Player {
    // ! Script event binding
    [SerializeField] GameManager gameManager;

    void Start() {
      gameManager.onLevelIncreased.AddListener(UpdateHealthOnNewLevel);
    }
  }
}
