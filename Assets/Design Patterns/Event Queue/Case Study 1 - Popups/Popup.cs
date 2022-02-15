using System;
using UnityEngine;

namespace EventQueuePattern.Case1.Base1 {
  public class Popup : MonoBehaviour {
    public Action OnClosed;

    public void Close() => OnClosed?.Invoke(); // bind to its button via the Inspector
  }
}