using System.Collections.Generic;

namespace EventQueuePattern.Case1.Base1 {
  public class CommandQueue {
    private readonly Queue<ICommand> _queue = new Queue<ICommand>();
    private bool _isPending; // it's true when a command is running

    public void Enqueue(ICommand command) {
      _queue.Enqueue(command);
      if (!_isPending) DoNext();
    }

    private void DoNext() {
      if (_queue.Count == 0) return;

      _isPending = true;
      var currentCommand = _queue.Dequeue();
      currentCommand.OnFinished += OnCurrentCommandFinished;
      currentCommand.Execute();
    }

    private void OnCurrentCommandFinished() {
      _isPending = false;
      DoNext();
    }
  }
}