namespace StatePattern.Base {
  public class State {
    public enum Stage {
      Enter,
      Update,
      Exit
    }

    protected State incommingState;

    protected Stage stage;

    public bool IsStageEnter => stage == Stage.Enter;
    public bool IsStageUpdate => stage == Stage.Update;
    public bool IsStageExit => stage == Stage.Exit;

    public virtual void Enter() {
      stage = Stage.Update;
    }

    public virtual void Update() {
      stage = Stage.Update;
    }

    public virtual void Exit() {
      stage = Stage.Exit;
    }

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