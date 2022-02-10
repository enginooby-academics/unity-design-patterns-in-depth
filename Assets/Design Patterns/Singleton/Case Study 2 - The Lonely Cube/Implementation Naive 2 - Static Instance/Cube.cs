using Shared = SingletonPattern.Case2;

// + same as Naive 1

namespace SingletonPattern.Case2.Naive2 {
  public class Cube : Shared.MonoBehaviourCube {
    public static Cube Instance { get; protected set; }

    private void Awake() => Instance = this;
  }
}