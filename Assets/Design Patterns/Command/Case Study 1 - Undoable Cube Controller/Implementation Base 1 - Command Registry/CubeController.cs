using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

// TODO
// + Boundary
// + Movement trait
// + Implement un-concurrent command
// + Implement UI controller to demonstrate duplicated implementation in naive impl

namespace CommandPattern.Case1.Base1 {
  /// <summary>
  ///   * The 'Invoker' class
  /// </summary>
  public class CubeController : MonoBehaviour {
    [SerializeField] private Cube _cube;

    private readonly List<MoveCommand> _commandHistory = new(); // ! can use Stack


    private void Start() => CommandRegistry.Instance.SetReceiver(_cube);

    private void Update() => CommandRegistry.Instance.Commands.ForEach(ProcessCommand);

    private void ProcessCommand(MoveCommand command) {
      if (!command.CanExecute) return;

      command.Execute();
      _commandHistory.Add(command);
    }

    [Button]
    [HorizontalGroup]
    public void Rewind() => StartCoroutine(RewindCoroutine());

    private IEnumerator RewindCoroutine() {
      // TIP: iterate collection reversely for removing operation
      foreach (var command in Enumerable.Reverse(_commandHistory)) {
        command.Undo();
        _commandHistory.Remove(command);
        yield return new WaitForSeconds(.5f);
      }
    }

    [Button]
    [HorizontalGroup]
    public void Undo() {
      if (_commandHistory.IsNullOrEmpty()) return;

      _commandHistory.GetLast().Undo();
      _commandHistory.RemoveLast();
    }
  }
}