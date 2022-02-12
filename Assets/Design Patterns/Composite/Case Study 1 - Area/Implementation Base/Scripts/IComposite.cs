using System.Collections.Generic;

namespace CompositePattern.Case1.Base {
  public interface IComposite<T> {
    List<T> Elements { get; }
    void Add(T element);

    void Remove(T element);
  }
}