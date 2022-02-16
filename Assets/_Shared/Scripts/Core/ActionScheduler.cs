using UnityEngine;

namespace Enginooby.Core {
  /// <summary>
  ///   Simplified version of State Pattern to manage action transition.
  ///   Used to implement actor controller of many actions.
  /// </summary>
  public class ActionScheduler : MonoBehaviour {
    private IAction currentAction;

    /// <summary>
    ///   Stop other actions than the given action.
    /// </summary>
    public void SwitchAction(IAction nextAction) {
      if (nextAction == currentAction) return;
      currentAction?.Cancel();
      currentAction = nextAction;
    }

    public void CancelCurrentAction() {
      currentAction?.Cancel();
    }
  }
}