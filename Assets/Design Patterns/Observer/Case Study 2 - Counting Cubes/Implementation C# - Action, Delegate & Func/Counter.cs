using System;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.CSharp {
  /// <summary>
  ///   * [The 'Subject' class]
  /// </summary>
  public class Counter : Shared.Counter {
    // ! Use static for easy referencing
    // ! Use keyword event to secure the event being invoked from outside the subject
    // ! 1: Use Delegate: can have return type
    public delegate string OnCountUpEventHandler(int number);

    public static event OnCountUpEventHandler OnCountUpDelegate;

    // ! 2: Use Func: can have return type
    public static event Func<int, string> OnCountUpFunc;

    // ! 3: Use C# Action: no return type
    public static event Action<int> OnCountUpAction;

    public override int Count {
      set {
        _count = value;
        OnCountUpDelegate?.Invoke(_count).Log();
        OnCountUpFunc?.Invoke(_count).Log();
        OnCountUpAction?.Invoke(_count);
      }
    }
  }
}