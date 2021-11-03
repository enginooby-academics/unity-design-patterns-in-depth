using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltEvents; // !

namespace ObserverPattern.UltEvents {
  public class GameManager : ObserverPattern.GameManager {
    public UltEvent<int> onLevelIncreasedEvent;

    protected override void OnLevelIncreasedCallback() {
      onLevelIncreasedEvent?.Invoke(_level);
    }
  }
}
