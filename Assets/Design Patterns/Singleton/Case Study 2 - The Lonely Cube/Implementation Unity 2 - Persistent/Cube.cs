using Shared = SingletonPattern.Case2;

// + unique
// + persistent

namespace SingletonPattern.Case2.Unity2 {
  public class Cube : Shared.MonoBehaviourCube {
    public static Cube Instance { get; protected set; }

    private void Awake() {
      if (Instance) {
        Destroy(gameObject);
      } else {
        Instance = this;
        DontDestroyOnLoad(gameObject); // !
      }
    }
  }
}