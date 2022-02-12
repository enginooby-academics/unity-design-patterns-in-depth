using System.Collections.Generic;

namespace CompositePattern.Case1.Base {
  public interface IAreaComposite : IArea {
    List<IArea> Areas { get; }
    void Add(IArea area);

    void Remove(IArea area);
  }
}