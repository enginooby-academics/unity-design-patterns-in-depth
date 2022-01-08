using UnityEngine;

namespace CommandPattern.Case1.Base1 {
  /// <summary>
  /// * The 'Abstract Command' class
  /// </summary>
  // ? Use singleton
  public abstract class MoveCommand {
    public Cube Cube;
    public KeyCode KeyCode { get; protected set; }


    public MoveCommand(Cube cube, KeyCode keyCode) {
      Cube = cube;
      KeyCode = keyCode;
    }

    public bool CanExecute => Input.GetKeyDown(KeyCode);
    public abstract void Execute();
    public abstract void Undo();
  }
}