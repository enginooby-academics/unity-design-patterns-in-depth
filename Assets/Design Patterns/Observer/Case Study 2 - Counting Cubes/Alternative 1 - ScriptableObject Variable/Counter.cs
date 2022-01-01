using UnityEngine;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Alternative1 {
  public class Counter : Shared.Counter {
    [SerializeField]
    private ReferenceIntSO _countVar;

    public override int Count { set => _count = _countVar.Value = value; }

    // Since SO data is persistent, manually reset when stop game
    private void OnApplicationQuit() => _countVar.Value = 0;
  }
}
