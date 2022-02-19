namespace AbstractDocumentPattern.Case2.Base1 {
  public interface IHasModel : IDocument {
#if UNITY_2021_2_OR_NEWER
    string GetModel() => (string) Get(Property.Model.ToString());
#endif
  }
}