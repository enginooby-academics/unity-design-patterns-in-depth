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

    public void Construct(MonoBehaviour monoBehaviour) {
      monoBehaviour.StartCoroutine(ConstructCoroutine(monoBehaviour));
    }

    // TODO: Create common static MonoBehaviour to start coroutines
    public IEnumerator ConstructCoroutine(MonoBehaviour monoBehaviour) {
      yield return monoBehaviour.StartCoroutine(_houseBuilder.BuildBase(_speed));
      yield return monoBehaviour.StartCoroutine(_houseBuilder.BuildRoof(_speed));
      yield return monoBehaviour.StartCoroutine(_houseBuilder.BuildDoor(_speed));
      yield return monoBehaviour.StartCoroutine(_houseBuilder.BuildWindows(_speed));
      yield return monoBehaviour.StartCoroutine(_houseBuilder.BuildChimney(_speed));
    }
  }
}