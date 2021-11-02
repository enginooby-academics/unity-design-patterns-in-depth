using UnityEngine;

public class ArchievedTriggerGameOver : ArchievedTrigger {
  public override void InvokeActionImpl() {
    GameManager.Instance.SetGameOver();//
  }

  private void Reset() {
    destroyAfterInvoked = DestroyTarget.Trigger;
  }
}
