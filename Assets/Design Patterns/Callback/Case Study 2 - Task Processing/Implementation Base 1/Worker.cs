using Enginooby.Attribute;
using UnityEngine;

namespace CallbackPattern.Case2.Base1 {
  /// <summary>
  /// The driver.
  /// </summary>
  public class Worker : MonoBehaviour {
    [SerializeField] private ReferenceConcreteType<ICallback> _taskCallbackType;

    [Button]
    private void ProcessNewTask() {
      var task = new Task();
      var taskCallback = _taskCallbackType.CreateInstance();
      task.Process(this, taskCallback);
    }
  }
}