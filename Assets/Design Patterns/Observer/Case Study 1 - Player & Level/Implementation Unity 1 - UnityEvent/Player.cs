namespace ObserverPattern3 {
  public class Player : ObserverPattern.Player {
    // ! Script event binding
    void Start() {
      GameManager.onLevelIncreasedEvent.AddListener(UpdateHealthOnNewLevel);

      // ! Cons: poor security - observer can modify or invoke UnityEvent of the subject
      // GameManager.onLevelIncreased.RemoveAllListeners();
      // GameManager.onLevelIncreased.Invoke();
    }
  }
}
