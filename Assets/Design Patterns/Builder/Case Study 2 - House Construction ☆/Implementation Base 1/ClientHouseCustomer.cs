using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace BuilderPattern.Case2.Base1 {
  public class ClientHouseCustomer : MonoBehaviour {
    [SerializeField] [HideLabel] private HomeContractor _homeContractor;

    [Button]
    public void BuyHouse() => _homeContractor.Construct();
  }
}