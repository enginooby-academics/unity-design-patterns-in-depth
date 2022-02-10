using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CommandPattern.Case1.CSharp1 {
  /// <summary>
  /// * The 'Invoker' class
  /// </summary>
  public class CubeController : MonoBehaviour {
    [SerializeField]
    private Cube _cube;

    private List<MoveCommand> _commands = new List<MoveCommand>();

    private List<MoveCommand> _commandHistory = new List<MoveCommand>(); // ! can use Stack


    void Start() {
      var moveUpCommand = new MoveCommand(
        keyCode: KeyCode.W,
        excuteFunc: delegate (Cube cube) {
          cube.MoveY(1f);
        },
        undoFunc: delegate (Cube cube) {
          cube.MoveY(-1f);
        }
      );

      var moveDownCommand = new MoveCommand(
        keyCode: KeyCode.S,
        excuteFunc: delegate (Cube cube) {
          cube.MoveY(-1f);
        },
        undoFunc: delegate (Cube cube) {
          cube.MoveY(1f);
        }
      );

      var moveLeftCommand = new MoveCommand(
        keyCode: KeyCode.A,
        excuteFunc: delegate (Cube cube) {
          cube.MoveX(-1f);
        },
        undoFunc: delegate (Cube cube) {
          cube.MoveX(1f);
        }
      );

      var moveRightCommand = new MoveCommand(
        keyCode: KeyCode.D,
        excuteFunc: delegate (Cube cube) {
          cube.MoveX(1f);
        },
        undoFunc: delegate (Cube cube) {
          cube.MoveX(-1);
        }
      );

      _commands = new List<MoveCommand>{
        moveUpCommand,
        moveDownCommand,
        moveLeftCommand,
        moveRightCommand,
      };
    }

    void Update() => _commands.ForEach(ProcessCommand);

    private void ProcessCommand(MoveCommand command) {
      if (!command.CanExecute) return;
      command.Execute(_cube);
      _commandHistory.Add(command);
    }

    [Button, HorizontalGroup]
    public void Rewind() => StartCoroutine(RewindCoroutine());

    public IEnumerator RewindCoroutine() {
      // TIP: iterate collection reversely
      foreach (var command in Enumerable.Reverse(_commandHistory)) {
        command.Undo(_cube);
        _commandHistory.Remove(command);
        yield return new WaitForSeconds(.5f);
      }
    }

    [Button, HorizontalGroup]
    public void Undo() {
      if (_commandHistory.IsUnset()) return;

      _commandHistory.GetLast().Undo(_cube);
      _commandHistory.RemoveLast();
    }
  }
}
