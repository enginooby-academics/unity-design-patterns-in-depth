using UnityEngine;

namespace ObserverPattern.Case2.Alternative2 {
  [CreateAssetMenu(fileName = "New Int Event", menuName = "Event/Int", order = 222)]
  public class IntEventSO : MonoParamEventSO<int> { }
}