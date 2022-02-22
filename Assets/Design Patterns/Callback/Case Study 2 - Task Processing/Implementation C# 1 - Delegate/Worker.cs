using Enginooby.Attribute;
using UnityEngine;

namespace CallbackPattern.Case2.Csharp1 {
  /// <summary>
  /// The driver.
  /// </summary>
  public class Worker : MonoBehaviour {
    private readonly Task _task = new();

    [Button]
    private void ProcessNewTask() {
      UseDelegate();
      // UseNormalFunction();
      // UseInlineFunction();
      // UseAnonymousFunction();
    }

    private void UseDelegate() {
      var taskCallback = new TaskCallbackDelegate(LogTask);
      _task.Process(this, taskCallback);
    }

    private void UseNormalFunction() {
      _task.Process(this, LogTask);
    }

    private void LogTask(float taskDuration) => Debug.Log($"The task took {taskDuration} seconds.");

    private void UseInlineFunction() {
      void TaskCallback(float taskDuration) => Debug.Log($"The task took {taskDuration} seconds.");
      _task.Process(this, TaskCallback);
    }

    private void UseAnonymousFunction() {
      _task.Process(this, taskDuration => Debug.Log($"The task took {taskDuration} seconds."));
    }

    // This requires to change TaskCallbackDelegate type in Task to Action<float>
    // private void UseAction() {
    //   Action<float> taskCallback = taskDuration => Debug.Log($"The task took {taskDuration} seconds.");
    //   task.Process(this, taskCallback);
    // }

    // Func must return value
  }
}