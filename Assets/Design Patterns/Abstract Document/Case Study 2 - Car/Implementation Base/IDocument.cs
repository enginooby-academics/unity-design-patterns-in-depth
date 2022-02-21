using System;
using System.Collections.Generic;

namespace AbstractDocumentPattern.Case2.Base1 {
  /// <summary>
  /// The document base interface <br/>
  /// Represent a property in the document class.
  /// </summary>
  public interface IDocument<TKey> {
    void Put(TKey key, object value);

    object Get(TKey key);

    IEnumerable<T> Children<T>(TKey key, Func<KeyValuePair<TKey, object>, T> constructor);
  }
}