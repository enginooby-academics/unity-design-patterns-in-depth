using UnityEngine;

namespace ObserverPattern.Case2.Alternative1 {
  [CreateAssetMenu(fileName = "New Int Variable", menuName = "Reference/Int", order = 0)]
  public class IntVariable : ScriptableObject {
    public int Value;
  }
}