using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern3 {
  public class Player : ObserverPattern.Player {
    // ! Script event binding
    [SerializeField] GameManager gameManager;

    void Start() {
      gameManager.onLevelIncreasedEvent.AddListener(UpdateHealthOnNewLevel);

      // ! Cons: poor security - observer can modify or invoke nityEvent of the subject
      // gameManager.onLevelIncreased.RemoveAllListeners();
      // gameManager.onLevelIncreased.Invoke();
    }
  }
}
