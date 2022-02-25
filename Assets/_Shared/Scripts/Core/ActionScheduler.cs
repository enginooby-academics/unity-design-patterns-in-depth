using UnityEngine;

namespace Enginooby.Core {
  /// <summary>
  ///   Simplified version of State Pattern to manage action transition.
  ///   Used to implement actor controller of many actions.
  /// </summary>
  public class ActionScheduler : MonoBehaviour {
    private IAction _currentAction;

    /// <summary>
    ///   Stop other actions than the given action.
    /// </summary>
    public void SwitchAction(IAction nextAction) {
      if (nextAction == _currentAction) return;

      _currentAction?.Cancel();
      _currentAction = nextAction;
    }

    public void CancelCurrentAction() {
      _currentAction?.Cancel();
    }
  }
}