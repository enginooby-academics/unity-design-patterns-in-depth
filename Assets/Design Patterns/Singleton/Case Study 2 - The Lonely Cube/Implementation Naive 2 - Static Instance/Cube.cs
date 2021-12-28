using Shared = SingletonPattern.Case2;

// + same as Naive 1

namespace SingletonPattern.Case2.Naive2 {
  public class Cube : Shared.Cube {
    public static Cube Instance;

    private void Awake() => Instance = this;
  }
}