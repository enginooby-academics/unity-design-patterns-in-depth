using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern.Base {
  public class State {
    public enum Stage {
      Enter, Update, Exit
    }

    public bool IsStageEnter => stage == Stage.Enter;
    public bool IsStageUpdate => stage == Stage.Update;
    public bool IsStageExit => stage == Stage.Exit;

    protected Stage stage;
    protected State incommingState;

    public virtual void Enter() { stage = Stage.Update; }
    public virtual void Update() { stage = Stage.Update; }
    public virtual void Exit() { stage = Stage.Exit; }

    public State Process() {
      if (IsStageEnter) Enter();
      if (IsStageUpdate) Update();
      if (IsStageExit) {
        Exit();
        return incommingState;
      }

      return this;
    }
  }
}