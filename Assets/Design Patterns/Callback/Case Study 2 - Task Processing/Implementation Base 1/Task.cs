using System.Collections;
using UnityEngine;

namespace CallbackPattern.Case2.Base1 {
  public class Task {
    public void Process(MonoBehaviour coroutineRunner, ICallback callback = null) {
      coroutineRunner.StartCoroutine(ProcessCoroutine(callback));
    }

    private IEnumerator ProcessCoroutine(ICallback callback) {
      Debug.Log("Start processing task.");
      var taskDuration = Random.Range(0f, 4f);
      yield return new WaitForSeconds(taskDuration);
      callback?.Execute(taskDuration);
    }
  }
}