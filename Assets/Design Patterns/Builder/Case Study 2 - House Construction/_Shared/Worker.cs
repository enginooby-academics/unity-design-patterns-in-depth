namespace BuilderPattern.Case2 {
  /// <summary>
  /// Common static MonoBehaviour to start coroutines in non-MonoBehaviour classes.
  /// </summary>
  public class Worker : MonoBehaviourSingleton<Worker> { }
}