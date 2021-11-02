using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // !

namespace ObserverPattern3 {
  public class GameManager : ObserverPattern.GameManager {
    public UnityEvent<int> onLevelIncreased = new UnityEvent<int>(); // !

    protected override void OnLevelIncreasedCallback() {
      onLevelIncreased.Invoke(_level); // !
    }
  }
}