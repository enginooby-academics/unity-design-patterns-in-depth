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
      _houseBuilder.StartCoroutine(ConstructCoroutine());
    }

    public IEnumerator ConstructCoroutine() {
      _houseBuilder.Container = new GameObject(_houseName);
      _houseBuilder.BuildBase();
      yield return new WaitForSeconds(100 / _speed);
      _houseBuilder.BuildRoof();
      yield return new WaitForSeconds(100 / _speed);
      _houseBuilder.BuildDoor();
      yield return new WaitForSeconds(100 / _speed);
      _houseBuilder.BuildWindows();
      yield return new WaitForSeconds(100 / _speed);
      _houseBuilder.BuildChymney();
    }
  }
}