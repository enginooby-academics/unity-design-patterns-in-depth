using UnityEngine;

namespace CallbackPattern.Case2.Base1 {
  /// <summary>
  /// * A concrete callback class. <br/>
  /// Simply log the duration of the task.
  /// </summary>
  public class TaskLogger : ICallback {
    public void Execute(float taskDuration) {
      Debug.Log($"The task took {taskDuration} seconds.");
    }
  }
}