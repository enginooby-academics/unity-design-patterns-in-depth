using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BuilderPattern.Case2.Naive1 {
  [System.Serializable, InlineProperty]
  public class HomeContractor {
    [SerializeField]
    [Range(50f, 300f)]
    [SuffixLabel("%")]
    private float _speed = 200f;

    [SerializeField, EnumToggleButtons]
    private House.Type _houseType;

    public void Construct() {
      StartCoroutine(ConstructCoroutine());
    }

    private Coroutine StartCoroutine(IEnumerator routine) {
      return Worker.Instance.StartCoroutine(routine);
    }

    public IEnumerator ConstructCoroutine() {
      House house = new House(_houseType);
      yield return StartCoroutine(house.BuildBase(_speed));
      yield return StartCoroutine(house.BuildRoof(_speed));
      yield return StartCoroutine(house.BuildDoor(_speed));
      yield return StartCoroutine(house.BuildWindows(_speed));
      yield return StartCoroutine(house.BuildChimney(_speed));
    }
  }
}