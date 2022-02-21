using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace CommandPattern.Case1.Unity1 {
  /// <summary>
  ///   * The 'Invoker' class
  /// </summary>
  public class CubeController : MonoBehaviour {
    [SerializeField] private Cube _cube;

    [SerializeField] private CommandRegistry _commandRegister;

    private readonly List<MoveCommand> _commandHistory = new(); // ! can use Stack


    private void Update() => _commandRegister.Commands.ForEach(ProcessCommand);

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