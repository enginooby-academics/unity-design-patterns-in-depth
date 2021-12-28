using Shared = SingletonPattern.Case2;

// + unique
// + persistance

namespace SingletonPattern.Case2.Unity2 {
  public class Cube : Shared.Cube {
    private static Cube _instance;
    public static Cube Instance => _instance;

    private void Awake() {
      if (Instance) {
        Destroy(gameObject);
      } else {
        _instance = this;
        DontDestroyOnLoad(gameObject); // !
      }
    }
  }
}