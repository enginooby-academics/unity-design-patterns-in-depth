using System.Collections;
using UnityEngine;

namespace BuilderPattern.Case2.Unity2 {
  [CreateAssetMenu(fileName = "New House Data", menuName = "Patterns/Builder/HouseData", order = 0)]
  public class HouseData : ScriptableObject {
    [SerializeField]
    private GameObject _base, _roof, _door, _windows, _chimney;

    [HideInInspector]
    public GameObject Container;

    private IEnumerator Build(GameObject part, float speed) {
      if (!part) yield break;

      var partGo = Instantiate(part);
      partGo.transform.SetParent(Container.transform);
      partGo.SetActive(true);
      yield return new WaitForSeconds(100 / speed);
    }

    // ? Necessary
    public IEnumerator BuildBase(float speed) => Build(_base, speed);
    public IEnumerator BuildRoof(float speed) => Build(_roof, speed);
    public IEnumerator BuildDoor(float speed) => Build(_door, speed);
    public IEnumerator BuildWindows(float speed) => Build(_windows, speed);
    public IEnumerator BuildChimney(float speed) => Build(_chimney, speed);
  }
}