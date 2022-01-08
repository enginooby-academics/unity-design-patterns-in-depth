using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern.Case1.Base1 {
  /// <summary>
  /// * The 'Abstract Command' class
  /// </summary>
  // ? Use singleton
  public abstract class MoveCommand {
    public Cube Cube;
    public KeyCode KeyCode;

    public MoveCommand(KeyCode keyCode) => KeyCode = keyCode;

    public bool CanExecute => Input.GetKeyDown(KeyCode);

    public abstract void Execute();
    public abstract void Undo();
  }

  /// <summary>
  /// Usually, we only need an instance of each Command type
  /// </summary>
  public class CommandRegister : MonoBehaviourSingleton<CommandRegister> {
    public List<MoveCommand> Commands;

    public override void AwakeSingleton() {
      Commands = TypeUtils.GetInstancesOf<MoveCommand>();
    }

    public void SetReceiver(Cube cube) {
      foreach (var command in Commands) {
        command.Cube = cube;
      }
    }

    public void SetKeyCodeFor<T>(KeyCode keyCode) where T : MoveCommand {
      GetCommand<T>().KeyCode = keyCode;
    }

    // TODO: exclude MoveCommand base class for T
    public MoveCommand GetCommand<T>() where T : MoveCommand {
      return Commands.Find(command => command.GetType() == typeof(T));
    }
  }
}