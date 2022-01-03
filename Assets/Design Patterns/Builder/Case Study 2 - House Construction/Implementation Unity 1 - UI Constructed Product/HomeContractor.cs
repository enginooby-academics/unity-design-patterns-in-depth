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
    private float _speed = 200f;

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
      yield return StartCoroutine(_houseBuilder.BuildBase(_speed));
      yield return StartCoroutine(_houseBuilder.BuildRoof(_speed));
      yield return StartCoroutine(_houseBuilder.BuildDoor(_speed));
      yield return StartCoroutine(_houseBuilder.BuildWindows(_speed));
      yield return StartCoroutine(_houseBuilder.BuildChimney(_speed));
    }
  }
}