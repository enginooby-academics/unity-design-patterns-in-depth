using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern3 {
  public class Player : ObserverPattern.Player {
    // ! Script event binding
    void Start() {
      GameManager.onLevelIncreasedEvent.AddListener(UpdateHealthOnNewLevel);

      // ! Cons: poor security - observer can modify or invoke nityEvent of the subject
      // GameManager.onLevelIncreased.RemoveAllListeners();
      // GameManager.onLevelIncreased.Invoke();
    }
  }
}
