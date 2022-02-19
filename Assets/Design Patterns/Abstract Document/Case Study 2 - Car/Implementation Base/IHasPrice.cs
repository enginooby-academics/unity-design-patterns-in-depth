namespace AbstractDocumentPattern.Case2.Base1 {
  public interface IHasPrice : IDocument {
#if UNITY_2021_2_OR_NEWER
    float? GetPrice() => (float?) Get(Property.Price.ToString());
#endif
  }
}