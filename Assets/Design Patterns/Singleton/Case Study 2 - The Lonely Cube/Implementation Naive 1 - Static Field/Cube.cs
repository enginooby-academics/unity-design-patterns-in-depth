using Shared = SingletonPattern.Case2;

// + eager init field
// ? scene-persistent
// + unique field

namespace SingletonPattern.Case2.Naive1 {
  public class Cube : MonoBehaviourCube {
    public static float StaticSize { get; private set; } = 3f;
  }
}