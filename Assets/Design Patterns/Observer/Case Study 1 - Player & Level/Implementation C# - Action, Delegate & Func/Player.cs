namespace ObserverPattern4 {
  public class Player : ObserverPattern.Player {
    // ! Ensure to bind event only when the observer is enabled
    private void OnEnable() {
      GameManager.onLevelIncreasedAction += UpdateHealthOnNewLevel;
    }

    private void OnDisable() {
      GameManager.onLevelIncreasedAction -= UpdateHealthOnNewLevel;
    }
  }
}