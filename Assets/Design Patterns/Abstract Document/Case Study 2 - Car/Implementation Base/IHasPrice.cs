namespace AbstractDocumentPattern.Case2.Base1 {
  public interface IHasPrice : IDocument {
    float? GetPrice() => (float?) Get(Property.Price.ToString());
  }
}