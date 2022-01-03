using UnityEngine;

namespace BuilderPattern.Case2.Unity1 {
  /// <summary>
  /// * The MonoBehaviour 'Builder' class
  /// </summary>
  public class HouseBuilder : MonoBehaviour {
    [SerializeField]
    private GameObject _base, _roof, _door, _windows, _chymney;

    public GameObject Container;

    private void Build(GameObject part) {
      if (!part) return;
      var partGo = Instantiate(part);
      partGo.transform.SetParent(Container.transform);
      partGo.SetActive(true);
    }

    public void BuildBase() => Build(_base);
    public void BuildRoof() => Build(_roof);
    public void BuildDoor() => Build(_door);
    public void BuildWindows() => Build(_windows);
    public void BuildChymney() => Build(_chymney);
  }
}
