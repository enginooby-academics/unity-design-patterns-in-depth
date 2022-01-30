using UnityEngine;

namespace ObserverPattern.Case2 {
  public abstract class Counter : MonoBehaviour {
    protected int _count;

    [HideInInspector] public virtual int Count { get => _count; set => _count = value; }
  }
}
