namespace AbstractDocumentPattern.Case2.Base1 {
  /// <summary>
  /// A "concrete" document interface.
  /// </summary>
  public interface IHasModel : IDocument<Property> {
#if UNITY_2021_2_OR_NEWER
    string GetModel() => (string) Get(Property.Model);
#endif
  }
}