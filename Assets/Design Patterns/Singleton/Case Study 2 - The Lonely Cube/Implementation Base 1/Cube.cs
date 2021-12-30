using Shared = SingletonPattern.Case2;

// + eager init field
// ? scene-persistent
// + unique: no

namespace SingletonPattern.Case2.Base1 {
  public class Cube : Shared.Cube {
    private static Cube _instance = null;
    // private static Cube _instance = new Cube();

    private Cube() {
      Init();
    }

    public static Cube Instance {
      get {
        if (_instance == null) {
          _instance = new Cube();
        }
        return _instance;
      }
    }
  }
}