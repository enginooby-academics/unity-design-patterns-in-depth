using UnityEngine;

namespace CommandPattern.Case1.CSharp1 {
  /// <summary>
  ///   * The 'Command' class
  /// </summary>
  public class MoveCommand {
    public delegate void Callback(Cube cube);

    public MoveCommand(KeyCode keyCode, Callback excuteFunc, Callback undoFunc) {
      KeyCode = keyCode;
      Execute = excuteFunc;
      Undo = undoFunc;
    }

    public KeyCode KeyCode { get; }
    public Callback Execute { get; }
    public Callback Undo { get; }

    public bool CanExecute => Input.GetKeyDown(KeyCode);
  }
}