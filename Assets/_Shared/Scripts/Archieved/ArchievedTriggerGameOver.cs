public class ArchievedTriggerGameOver : ArchievedTrigger {
  private void Reset() {
    destroyAfterInvoked = DestroyTarget.Trigger;
  }

  public override void InvokeActionImpl() {
    GameManager.Instance.SetGameOver(); //
  }
}