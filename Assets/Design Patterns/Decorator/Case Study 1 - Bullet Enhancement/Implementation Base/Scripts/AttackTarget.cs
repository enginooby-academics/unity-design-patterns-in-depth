using UnityEngine;

public class AttackTarget : MonoBehaviour {
  public void GetHit(float damage) {
    print($"Target receive {damage} damage.");
  }

  public void GetBurned() {
    print("Target is burned.");
  }
}
