using System;
using System.Collections;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

namespace BuilderPattern.Case2.Naive1 {
  [Serializable]
  [InlineProperty]
  public class HomeContractor {
    [SerializeField] [Range(50f, 300f)] [SuffixLabel("%")]
    private float _speed = 200f;

    [SerializeField] [EnumToggleButtons] private House.Type _houseType;

    public void Construct() => StartCoroutine(ConstructCoroutine());

    private Coroutine StartCoroutine(IEnumerator routine) => Worker.Instance.StartCoroutine(routine);

    public IEnumerator ConstructCoroutine() {
      var house = new House(_houseType);
      yield return StartCoroutine(house.BuildBase(_speed));
      yield return StartCoroutine(house.BuildRoof(_speed));
      yield return StartCoroutine(house.BuildDoor(_speed));
      yield return StartCoroutine(house.BuildWindows(_speed));
      yield return StartCoroutine(house.BuildChimney(_speed));
    }
  }
}