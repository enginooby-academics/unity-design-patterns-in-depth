using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CallbackPattern.Case2.Csharp1 {
  public delegate void TaskCallbackDelegate(float taskDuration);

  public class Task {
    public void Process(MonoBehaviour coroutineRunner, TaskCallbackDelegate callback = null) {
      coroutineRunner.StartCoroutine(ProcessCoroutine(callback));
    }

    private IEnumerator ProcessCoroutine(TaskCallbackDelegate callback) {
      Debug.Log("Start processing task.");
      var taskDuration = Random.Range(0f, 4f);
      yield return new WaitForSeconds(taskDuration);
      callback?.Invoke(taskDuration);
    }
  }
}