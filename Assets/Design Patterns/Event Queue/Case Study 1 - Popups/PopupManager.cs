using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace EventQueuePattern.Case1.Base1 {
  public class PopupManager : MonoBehaviourSingleton<PopupManager> {
    private readonly CommandQueue _commandQueue = new CommandQueue();

    [field: SerializeField] public Popup FirstPopup { get; [UsedImplicitly] private set; }
    [field: SerializeField] public Popup SecondPopup { get; [UsedImplicitly] private set; }
    [field: SerializeField] public Popup ThirdPopup { get; [UsedImplicitly] private set; }

    private void Start() => StartCoroutine(EnqueueCommandsCoroutine());

    private IEnumerator EnqueueCommandsCoroutine() {
      yield return new WaitForSeconds(1f);

      _commandQueue.Enqueue(new FirstCommand());
      _commandQueue.Enqueue(new SecondCommand());
      _commandQueue.Enqueue(new ThirdCommand());
    }
  }
}