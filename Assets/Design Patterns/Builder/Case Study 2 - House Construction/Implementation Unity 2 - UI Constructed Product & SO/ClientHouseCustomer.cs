using Sirenix.OdinInspector;
using UnityEngine;

namespace BuilderPattern.Case2.Unity2 {
  public class ClientHouseCustomer : MonoBehaviour {
    [SerializeField, InlineEditor]
    private HomeContractorData _homeContractor;

    [Button]
    public void BuyHouse() => _homeContractor.Construct(this);
  }
}