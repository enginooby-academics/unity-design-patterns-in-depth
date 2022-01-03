using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BuilderPattern.Case2.Unity1 {
  /// <summary>
  /// * The 'Director' class (optional)
  /// </summary>
  public class HomeContractor : MonoBehaviour {
    [SerializeField]
    [Range(50f, 300f)]
    [SuffixLabel("%")]
    private float speed = 200f;

    [SerializeField]
    private HouseBuilder _houseBuilder;

    [SerializeField]
    private string _houseName = "House";

    [Button]
    public void Construct() {
      _houseBuilder.Container = new GameObject(_houseName);
      StartCoroutine(ConstructCoroutine());
    }

    public IEnumerator ConstructCoroutine() {
      _houseBuilder.BuildBase();
      yield return new WaitForSeconds(100 / speed);
      _houseBuilder.BuildRoof();
      yield return new WaitForSeconds(100 / speed);
      _houseBuilder.BuildDoor();
      yield return new WaitForSeconds(100 / speed);
      _houseBuilder.BuildWindows();
      yield return new WaitForSeconds(100 / speed);
      _houseBuilder.BuildChymney();
    }
  }
}