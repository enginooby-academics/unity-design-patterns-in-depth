using UnityEngine;

namespace ObserverPattern.Case2.Alternative2 {
  [CreateAssetMenu(fileName = "New Int Event", menuName = "Event/Int", order = 0)]
  public class IntEventSO : OneVarEventSO<int> { }
}