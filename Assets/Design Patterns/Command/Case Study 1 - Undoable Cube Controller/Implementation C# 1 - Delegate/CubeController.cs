using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

namespace CommandPattern.Case1.CSharp1 {
  /// <summary>
  ///   * The 'Invoker' class
  /// </summary>
  public class CubeController : MonoBehaviour {
    [SerializeField] private Cube _cube;

    private readonly List<MoveCommand> _commandHistory = new(); // ! can use Stack

    private List<MoveCommand> _commands = new();


    private void Start() {
      var moveUpCommand = new MoveCommand(
        KeyCode.W,
        delegate(Cube cube) { cube.MoveY(1f); },
        delegate(Cube cube) { cube.MoveY(-1f); }
      );

      var moveDownCommand = new MoveCommand(
        KeyCode.S,
        delegate(Cube cube) { cube.MoveY(-1f); },
        delegate(Cube cube) { cube.MoveY(1f); }
      );

      var moveLeftCommand = new MoveCommand(
        KeyCode.A,
        delegate(Cube cube) { cube.MoveX(-1f); },
        delegate(Cube cube) { cube.MoveX(1f); }
      );

      var moveRightCommand = new MoveCommand(
        KeyCode.D,
        delegate(Cube cube) { cube.MoveX(1f); },
        delegate(Cube cube) { cube.MoveX(-1); }
      );

      _commands = new List<MoveCommand> {
        moveUpCommand,
        moveDownCommand,
        moveLeftCommand,
        moveRightCommand,
      };
    }

    private void Update() => _commands.ForEach(ProcessCommand);

    private void ProcessCommand(MoveCommand command) {
      if (!command.CanExecute) return;
      command.Execute(_cube);
      _commandHistory.Add(command);
    }

    [Button]
    [HorizontalGroup]
    public void Rewind() => StartCoroutine(RewindCoroutine());

    public IEnumerator RewindCoroutine() {
      // TIP: iterate collection reversely
      foreach (var command in Enumerable.Reverse(_commandHistory)) {
        command.Undo(_cube);
        _commandHistory.Remove(command);
        yield return new WaitForSeconds(.5f);
      }
    }

    [Button]
    [HorizontalGroup]
    public void Undo() {
      if (_commandHistory.IsNullOrEmpty()) return;

      _commandHistory.GetLast().Undo(_cube);
      _commandHistory.RemoveLast();
    }
  }
}