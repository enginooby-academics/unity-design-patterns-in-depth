using UnityEngine;

namespace CommandPattern.Case1.Base1 {
  /// <summary>
  /// * A 'Concrete Command' class
  /// </summary>
  public class MoveLeftCommand : MoveCommand {
    public MoveLeftCommand() : base(KeyCode.A) { }

    public override void Execute() => Cube.MoveX(-1f);

    public override void Undo() => Cube.MoveX(1f);

  }
}