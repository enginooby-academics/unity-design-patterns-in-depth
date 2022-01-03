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
    private float speed = 200f;

    [SerializeReference]
    private IHouseBuilder _houseBuilder;

    [Button]
    public void Construct() {
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