using UnityEngine;

namespace ObserverPattern.Case2 {
  public class Counter : MonoBehaviourSingleton<Counter> {
    protected int _count;

    [HideInInspector] public virtual int Count { get => _count; set => _count = value; }
  }
}
