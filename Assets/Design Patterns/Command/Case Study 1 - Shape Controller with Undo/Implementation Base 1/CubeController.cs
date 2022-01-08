using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

// TODO
// + Switch receiver
// + Boundary
// + Movement trait
// + Implement un-concurrent command

// + Implement UI controller to demonstrate duplicated implementation in naive impl
namespace CommandPattern.Case1.Base1 {
  /// <summary>
  /// * The 'Invoker' class
  /// </summary>
  public class CubeController : MonoBehaviour {
    [SerializeField]
    private Cube _cube;

    private List<MoveCommand> _cubeCommands = new List<MoveCommand>(); // ? create static instances
    private List<MoveCommand> _commandHistory = new List<MoveCommand>(); // ! can use Stack

    void Start() {
      _cubeCommands = new List<MoveCommand> {
        new MoveUpCommand(_cube, KeyCode.W),
        new MoveDownCommand(_cube, KeyCode.S),
        new MoveLeftCommand(_cube, KeyCode.A),
        new MoveRightCommand(_cube, KeyCode.D),
      };
    }

    void Update() => _cubeCommands.ForEach(ProcessCommand);

    private void ProcessCommand(MoveCommand command) {
      if (!command.CanExecute) return;
      command.Execute();
      _commandHistory.Add(command);
    }

    [Button]
    public void Rewind() => StartCoroutine(RewindCoroutine());

    public IEnumerator RewindCoroutine() {
      // TIP: iterate collection reversely
      foreach (var command in Enumerable.Reverse(_commandHistory)) {
        command.Undo();
        _commandHistory.Remove(command);
        yield return new WaitForSeconds(.5f);
      }
    }

    [Button]
    public void Undo() {
      if (_commandHistory.IsUnset()) return;

      _commandHistory.GetLast().Undo();
      _commandHistory.RemoveLast();
    }
  }
}
