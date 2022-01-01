using UnityEngine;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Alternative1 {
  public class Counter : Shared.Counter {
    [SerializeField]
    private new ReferenceIntSO _count;

    public override int Count { get => _count.Value; set => _count.Value = value; }
  }
}
