using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.CSharp {
  /// <summary>
  /// * [The 'Subject' class]
  /// </summary>
  public class Counter : Shared.Counter {
    // ! 1: Use C# Action: no return type
    // ! Use keyword event to secure the action being invoked from outside the subject
    public static event System.Action<int> OnCountUpEvent;

    // ! 2: Use Delegate: can have return type
    // public delegate string OnCountUpCallBack(int number);
    // public static event OnCountUpCallBack OnCountUpEvent;

    // ! 3: Use Func: can have return type
    // public static System.Func<int, string> OnCountUpEvent;

    public override int Count {
      set {
        _count = value;
        OnCountUpEvent?.Invoke(_count);
        // OnCountUpEvent?.Invoke(_count).Log();
      }
    }
  }
}
