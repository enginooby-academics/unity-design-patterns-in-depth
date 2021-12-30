using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events; // !

namespace ObserverPattern3 {
  public class GameManager : ObserverPattern.GameManager {
    [ShowInInspector]
    [SerializeField]
    public static UnityEvent<int> onLevelIncreasedEvent = new UnityEvent<int>(); // !

    protected override void OnLevelIncreasedCallback() {
      onLevelIncreasedEvent.Invoke(_level); // !
    }
  }
}