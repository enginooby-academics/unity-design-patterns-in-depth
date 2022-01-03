using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BuilderPattern.Case2.Base1 {
  /// <summary>
  /// * The 'Director' class (optional)
  /// </summary>
  public class HomeContractor : MonoBehaviour {
    [SerializeField]
    [Range(50f, 300f)]
    [SuffixLabel("%")]
    private float _speed = 200f;

    [SerializeReference]
    private IHouseBuilder _houseBuilder;

    [Button]
    public void Construct() {
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