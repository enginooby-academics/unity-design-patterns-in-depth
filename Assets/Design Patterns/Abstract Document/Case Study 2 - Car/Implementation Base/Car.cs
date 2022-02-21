using System.Collections.Generic;
using JetBrains.Annotations;

namespace AbstractDocumentPattern.Case2.Base1 {
#if UNITY_2021_2_OR_NEWER
  public class Car : AbstractDocument<Property>, IHasPrice, IHasModel {
#else
  public class Car : AbstractDocument {
#endif
    public Car([NotNull] Dictionary<Property, object> properties) : base(properties) { }
    public Car() { }
  }
}