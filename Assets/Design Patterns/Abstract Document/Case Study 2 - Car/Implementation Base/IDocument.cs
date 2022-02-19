using System;
using System.Collections.Generic;

namespace AbstractDocumentPattern.Case2.Base1 {
  public interface IDocument {
    void Put(string key, object value);

    void Put(IFormattable key, object value);

    object Get(string key);

    object Get(IFormattable key);

    IEnumerable<T> Children<T>(string key, Func<KeyValuePair<string, object>, T> constructor);
  }
}