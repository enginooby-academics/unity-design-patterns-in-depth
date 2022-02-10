#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using UnityEngine;

namespace BuilderPattern.Case2.Unity2 {
  public class ClientHouseCustomer : MonoBehaviour {
    [SerializeField, InlineEditor]
    private HomeContractorData _homeContractor;

    [Button]
    public void BuyHouse() => _homeContractor.Construct();
  }
}