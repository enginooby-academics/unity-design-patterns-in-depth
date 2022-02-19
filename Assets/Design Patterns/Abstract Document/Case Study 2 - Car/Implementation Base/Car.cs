using System.Collections.Generic;
using JetBrains.Annotations;

namespace AbstractDocumentPattern.Case2.Base1 {
  public class Car : AbstractDocument, IHasPrice, IHasModel {
    public Car([NotNull] Dictionary<string, object> properties) : base(properties) { }
    public Car() { }
  }
}