using System;

namespace IteratorPattern.Case1.Base {
  public interface IIterable<T> where T : class {
    IIterator<T> GetIterator();
  }

  [Serializable]
  /// <summary>
  /// Interface wrapper to serialize in the Inspector.
  /// </summary>
  public class IIterableContainer<T> : IUnifiedContainer<IIterable<T>> where T : class { }
}
