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

namespace CommandPattern.Case1.Naive1 {
  /// <summary>
  /// * The 'Invoker' class
  /// </summary>
  public class CubeController : MonoBehaviour {
    [SerializeField]
    private Cube _cube;

    public enum Command { MoveUp, MoveDown, MoveLeft, MoveRight }
    private List<Command> _commandHistory = new List<Command>();

    void Update() {
      if (Input.GetKeyDown(KeyCode.W)) {
        _cube.MoveY(1f);
        _commandHistory.Add(Command.MoveUp);
      }
      if (Input.GetKeyDown(KeyCode.S)) {
        _cube.MoveY(-1f);
        _commandHistory.Add(Command.MoveDown);
      }
      if (Input.GetKeyDown(KeyCode.A)) {
        _cube.MoveX(-1f);
        _commandHistory.Add(Command.MoveLeft);
      }
      if (Input.GetKeyDown(KeyCode.D)) {
        _cube.MoveX(1f);
        _commandHistory.Add(Command.MoveRight);
      }
    }

    [Button]
    public void Rewind() => StartCoroutine(RewindCoroutine());

    // = Undo all
    public IEnumerator RewindCoroutine() {
      foreach (var command in Enumerable.Reverse(_commandHistory)) {
        UndoCommand(command);
        yield return new WaitForSeconds(.5f);
      }
    }

    private void UndoCommand(Command command) {
      switch (command) {
        case Command.MoveUp:
          _cube.MoveY(-1f);
          break;
        case Command.MoveDown:
          _cube.MoveY(1f);
          break;
        case Command.MoveLeft:
          _cube.MoveX(1f);
          break;
        case Command.MoveRight:
          _cube.MoveX(-1f);
          break;
      }
      _commandHistory.Remove(command);
    }

    [Button]
    public void Undo() {
      if (_commandHistory.IsUnset()) return;

      UndoCommand(_commandHistory.GetLast());
    }
  }
}
