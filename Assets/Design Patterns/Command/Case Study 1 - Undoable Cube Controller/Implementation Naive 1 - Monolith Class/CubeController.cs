using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enginooby.Utils;
using UnityEngine;
using UnityEngine.UI;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace CommandPattern.Case1.Naive1 {
  public enum Command {
    MoveUp,
    MoveDown,
    MoveLeft,
    MoveRight,
  }

  public class CubeController : MonoBehaviour {
    private static readonly List<Command> _commandHistory = new();
    [SerializeField] private Cube _cube;
    [SerializeField] private Button _moveUpButton;

    private void Update() {
      ProcessCommands();
    }

    private void OnEnable() {
      _moveUpButton.onClick.AddListener(() => {
        _cube.MoveY(1f);
        _commandHistory.Add(Command.MoveUp);
      });
    }

    private void ProcessCommands() {
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
    [HorizontalGroup]
    public void Rewind() => StartCoroutine(RewindCoroutine());

    [Button]
    [HorizontalGroup]
    public void Undo() => UndoACommand(_commandHistory.GetLast());

    // = Undo all
    public IEnumerator RewindCoroutine() {
      foreach (var command in Enumerable.Reverse(_commandHistory)) {
        UndoACommand(command);
        yield return new WaitForSeconds(.5f);
      }
    }

    private void UndoACommand(Command command) {
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
  }
}