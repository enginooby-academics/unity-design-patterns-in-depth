using UnityEngine.Events;

public static class EventUtils {
  public static void AddListenerTernary(
    this UnityEvent @event,
    bool condition,
    UnityAction trueAction,
    UnityAction falseAction) {
    @event.AddListener(condition ? trueAction : falseAction);
  }
}