using System.Collections.Generic;

namespace CompositePattern.Case1.Base {
  public interface IAreaComposite : IArea {
    void Add(IArea area);

    void Remove(IArea area);

    List<IArea> Areas { get; }
  }
}
