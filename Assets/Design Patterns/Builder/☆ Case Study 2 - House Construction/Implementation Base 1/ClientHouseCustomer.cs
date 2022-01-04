using Sirenix.OdinInspector;
using UnityEngine;

namespace BuilderPattern.Case2.Base1 {
  public class ClientHouseCustomer : MonoBehaviour {
    [SerializeField, HideLabel]
    private HomeContractor _homeContractor;

    [Button]
    public void BuyHouse() => _homeContractor.Construct();
  }
}