using Shared = SingletonPattern.Case2;

// + same as Naive 1

namespace SingletonPattern.Case2.Naive2 {
  public class Cube : Shared.Cube {
    private static Cube _instance;
    public static Cube Instance => _instance;

    private void Awake() => _instance = this;
  }
}