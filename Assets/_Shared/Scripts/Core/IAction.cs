using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enginoobz.Core {
  /// <summary>
  /// Simplified version of State Pattern. Used to implement controller of many actions for a actor.
  /// </summary>
  public interface IAction {
    void Cancel();
  }
}
