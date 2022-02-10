#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using UnityEngine;

namespace BuilderPattern.Case2.Unity1 {
  public class ClientHouseCustomer : MonoBehaviour {
    [SerializeField]
    private HomeContractor _homeContractor;

    [Button]
    public void BuyHouse() => _homeContractor.Construct();
  }
}