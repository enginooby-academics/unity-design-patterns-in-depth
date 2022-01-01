using Sirenix.Serialization;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Alternative1 {
  public class Counter : Shared.Counter {
    [OdinSerialize]
    public ReferenceIntSO CountRef { get; private set; }

    public override int Count { get => CountRef.Value; set => CountRef.Value = value; }
  }
}
