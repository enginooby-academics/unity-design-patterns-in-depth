using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // !

namespace ObserverPattern3 {
  public class GameManager : ObserverPattern.GameManager {
    public UnityEvent<int> onLevelIncreasedEvent = new UnityEvent<int>(); // !

    protected override void OnLevelIncreasedCallback() {
      onLevelIncreasedEvent.Invoke(_level); // !
    }
  }
}