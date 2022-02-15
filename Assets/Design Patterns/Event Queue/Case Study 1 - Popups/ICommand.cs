using System;

namespace EventQueuePattern.Case1.Base1 {
  public interface ICommand {
    Action OnFinished { get; set; }

    void Execute();
  }
}