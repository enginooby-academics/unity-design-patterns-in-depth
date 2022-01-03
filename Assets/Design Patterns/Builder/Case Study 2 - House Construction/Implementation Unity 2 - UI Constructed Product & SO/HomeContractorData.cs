using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BuilderPattern.Case2.Unity2 {
  [CreateAssetMenu(fileName = "New Home Contractor Data", menuName = "Patterns/Builder/HomeContractorData", order = 0)]
  public class HomeContractorData : ScriptableObject {
    [SerializeField]
    [Range(50f, 300f)]
    [SuffixLabel("%")]
    private float _speed = 200f;

    [SerializeField, InlineEditor]
    private HouseData _houseData;

    [SerializeField]
    private string _houseName = "House";

    public void Construct(MonoBehaviour monoBehaviour) {
      monoBehaviour.StartCoroutine(ConstructCoroutine(monoBehaviour));
    }

    public IEnumerator ConstructCoroutine(MonoBehaviour monoBehaviour) {
      _houseData.Container = new GameObject(_houseName);
      yield return monoBehaviour.StartCoroutine(_houseData.BuildBase(_speed));
      yield return monoBehaviour.StartCoroutine(_houseData.BuildRoof(_speed));
      yield return monoBehaviour.StartCoroutine(_houseData.BuildDoor(_speed));
      yield return monoBehaviour.StartCoroutine(_houseData.BuildWindows(_speed));
      yield return monoBehaviour.StartCoroutine(_houseData.BuildChimney(_speed));
    }
  }
}