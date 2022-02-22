using UnityEngine;

namespace CallbackPattern.Case2.Base1 {
  /// <summary>
  /// * A concrete callback class. <br/>
  /// Give feedback and grade base on the task duration.
  /// </summary>
  public class TaskGrader : ICallback {
    public void Execute(float taskDuration) {
      switch (taskDuration) {
        case < 1f:
          Debug.Log("Excellent! You got A+.");
          break;
        case < 3f:
          Debug.Log("Good job! You got B");
          break;
        default:
          Debug.Log("Nice try! You got C");
          break;
      }
    }
  }
}