namespace IteratorPattern.Case1.Base {
  public interface IIterator<T> where T : class {
    T GetNext();
    bool HasNext();
  }
}