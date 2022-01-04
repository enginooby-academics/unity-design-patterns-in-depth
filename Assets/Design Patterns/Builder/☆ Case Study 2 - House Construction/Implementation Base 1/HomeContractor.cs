using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BuilderPattern.Case2.Base1 {
  /// <summary>
  /// * The 'Director' class (optional)
  /// </summary>
  [System.Serializable, InlineProperty]
  public class HomeContractor {
    [SerializeField]
    [Range(50f, 300f)]
    [SuffixLabel("%")]
    private float _speed = 200f;

    [SerializeReference]
    private IHouseBuilder _houseBuilder;

    public void Construct() {
      StartCoroutine(ConstructCoroutine());
    }

    private Coroutine StartCoroutine(IEnumerator routine) {
      return Worker.Instance.StartCoroutine(routine);
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