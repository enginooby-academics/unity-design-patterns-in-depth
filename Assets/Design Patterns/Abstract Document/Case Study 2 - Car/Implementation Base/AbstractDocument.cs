using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace AbstractDocumentPattern.Case2.Base1 {
  /// <summary>
  /// * The abstract document base class <br/>
  /// Object of this type holds a dynamic list of fields.
  /// </summary>
  public abstract class AbstractDocument<TKey> : IDocument<TKey> {
    private readonly Dictionary<TKey, object> _properties = new();

    protected AbstractDocument() { }

    protected AbstractDocument([NotNull] Dictionary<TKey, object> properties) =>
      _properties = properties ?? throw new ArgumentNullException(nameof(properties));

    public void Put(TKey key, object value) => _properties.Add(key, value);

    public object Get(TKey key) => _properties.GetValueOrDefault(key);

    public IEnumerable<T> Children<T>(TKey key, Func<KeyValuePair<TKey, object>, T> constructor) {
      var children = Get(key) as Dictionary<TKey, object>;
      return children is null ? Enumerable.Empty<T>() : children.Select(constructor.Invoke);

      // List<T> theChildren = null;
      // foreach (Dictionary<string, object> child in children) {
      //   theChildren.Add(constructor(child));
      // }
      //
      // return theChildren;
    }
  }
}