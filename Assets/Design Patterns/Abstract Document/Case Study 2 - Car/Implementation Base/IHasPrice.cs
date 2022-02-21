namespace AbstractDocumentPattern.Case2.Base1 {
  /// <summary>
  /// A "concrete" document interface.
  /// </summary>
  public interface IHasPrice : IDocument<Property> {
#if UNITY_2021_2_OR_NEWER
    float? GetPrice() => (float?) Get(Property.Price);
#endif
  }
}