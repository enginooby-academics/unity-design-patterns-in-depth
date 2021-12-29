using Shared = SingletonPattern.Case2;

// + eager init field
// ? scene-persistent
// + unique field

namespace SingletonPattern.Case2.Naive1 {
  public class Cube : Shared.MonoBehaviourCube {
    public static float StaticSize { get; protected set; }

    private void Awake() => Size = StaticSize = 3f;
  }
}