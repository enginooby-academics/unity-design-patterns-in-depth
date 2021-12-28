using Shared = SingletonPattern.Case2;

// + eager init field
// ? scene-persistent
// + unique: no

namespace SingletonPattern.Case2.Naive1 {
  public class Cube : Shared.Cube {
    private static float _staticSize = 3f;
    public static float StaticSize => _staticSize;

    private void Awake() => _size = _staticSize;
  }
}