using UnityEngine;

namespace CommandPattern.Case1.Base1 {
  /// <summary>
  /// * A 'Concrete Command' class
  /// </summary>
  public class MoveUpCommand : MoveCommand {
    public MoveUpCommand() : base(KeyCode.W) { }

    public override void Execute() => Cube.MoveY(1f);

    public override void Undo() => Cube.MoveY(-1f);
  }
}