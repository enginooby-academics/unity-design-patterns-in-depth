#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

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