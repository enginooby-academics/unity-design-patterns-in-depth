using System.Collections;
namespace BuilderPattern.Case2.Base1 {
  /// <summary>
  /// * The 'Builder' contract
  /// </summary>
  public interface IHouseBuilder {
    IEnumerator BuildBase(float speed);
    IEnumerator BuildRoof(float speed);
    IEnumerator BuildDoor(float speed);
    IEnumerator BuildWindows(float speed);
    IEnumerator BuildChimney(float speed);
  }
}
