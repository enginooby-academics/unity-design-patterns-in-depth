using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ArchievedTriggerScore : ArchievedTriggerStat<int> {
  // TIP: Reset() is invoked when Component first added or being Reset (in Inspector)
  // Hence useful for ovveride defaut values of base class 
  public override string StatLabel { get => "Score amount"; }
  private void Reset() {
    statValue = 1;
    triggerEvent = TriggerEventType.OnTriggerEnter;
  }

  public override void UpdateStat() {
    GameManager.Instance.UpdateScores(statValue);
  }
}
