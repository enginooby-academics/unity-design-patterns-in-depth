using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern4 {
  public class GameManager : ObserverPattern.GameManager {
    // ! 1: Use C# Action: no return type
    // ! Use keyword event to secure the action being invoked from outside the subject
    public static event System.Action<int> onLevelIncreasedAction;

    // ! 2: Use Delegate: can have return type
    // public delegate string CallbackType(int level);
    // public event CallbackType onLevelIncreasedAction;

    // ! 3: Use Func: can have return type
    // public System.Func<int, string> onLevelIncreasedAction;

    protected override void OnLevelIncreasedCallback() {
      onLevelIncreasedAction?.Invoke(_level);
    }
  }
}
