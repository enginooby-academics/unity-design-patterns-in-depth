namespace AbstractDocumentPattern.Case2.Base1 {
  public interface IHasModel : IDocument {
    string GetModel() => (string) Get(Property.Model.ToString());
  }
}