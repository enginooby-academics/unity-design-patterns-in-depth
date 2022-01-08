using UnityEngine;

namespace CommandPattern.Case1.Base1 {
  /// <summary>
  /// * A 'Concrete Command' class
  /// </summary>
  public class MoveRightCommand : MoveCommand {
    public MoveRightCommand() : base(KeyCode.D) { }

    public override void Execute() => Cube.MoveX(1f);

    public override void Undo() => Cube.MoveX(-1f);
  }
}
