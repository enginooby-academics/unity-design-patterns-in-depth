using UnityEngine;

namespace CommandPattern.Case1.CSharp1 {
  /// <summary>
  /// * The 'Command' class
  /// </summary>
  public class MoveCommand {
    public KeyCode KeyCode { get; private set; }
    public Callback Execute { get; private set; }
    public Callback Undo { get; private set; }
    public delegate void Callback(Cube cube);

    public bool CanExecute => Input.GetKeyDown(KeyCode);

    public MoveCommand(KeyCode keyCode, Callback excuteFunc, Callback undoFunc) {
      KeyCode = keyCode;
      Execute = excuteFunc;
      Undo = undoFunc;
    }
  }
}