using System.Collections.Generic;

namespace CompositePattern.Case1.Base {
  public interface IComposite<T> {
    void Add(T element);

    void Remove(T element);

    List<T> Elements { get; }
  }
}
