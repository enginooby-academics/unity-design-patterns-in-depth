using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

namespace BuilderPattern.Case2.Naive2 {
  public class ClientHouseCustomer : MonoBehaviour {
    [SerializeField] [HideLabel] private HomeContractor _homeContractor;

    [Button]
    public void BuyHouse() => _homeContractor.Construct();
  }
}