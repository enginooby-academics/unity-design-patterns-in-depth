using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace AbstractDocumentPattern.Case2.Base1 {
  public abstract class AbstractDocument : IDocument {
    private readonly Dictionary<string, object> _properties = new();

    protected AbstractDocument() { }

    protected AbstractDocument([NotNull] Dictionary<string, object> properties) =>
      _properties = properties ?? throw new ArgumentNullException(nameof(properties));

    public void Put(string key, object value) => _properties.Add(key, value);

    public void Put(IFormattable key, object value) => Put(key.ToString(), value);

    public object Get(string key) => _properties.GetValue(key);

    public object Get(IFormattable key) => Get(key.ToString());

    public IEnumerable<T> Children<T>(string key, Func<KeyValuePair<string, object>, T> constructor) {
      var children = Get(key) as Dictionary<string, object>;
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