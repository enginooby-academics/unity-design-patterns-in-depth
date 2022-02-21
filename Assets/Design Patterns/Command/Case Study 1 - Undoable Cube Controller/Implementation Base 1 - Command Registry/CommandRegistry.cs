using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CommandPattern.Case1.Base1 {
  /// <summary>
  ///   Usually, we only need an instance of each Command type.
  /// </summary>
  // ! Can be non-MonoBehaviour singleton
  public class CommandRegistry : MonoBehaviourSingleton<CommandRegistry> {
    public IEnumerable<MoveCommand> Commands;

    public override void AwakeSingleton() => Commands = TypeUtils.CreateInstancesOf<MoveCommand>();

    public void SetReceiver(Cube cube) {
      foreach (var command in Commands) command.Cube = cube;
    }

    public void SetKeyCodeFor<T>(KeyCode keyCode) where T : MoveCommand => GetCommand<T>().KeyCode = keyCode;

    // TODO: exclude MoveCommand base class for T
    public MoveCommand GetCommand<T>() where T : MoveCommand {
      return Commands.FirstOrDefault(command => command.GetType() == typeof(T));
    }
  }
}