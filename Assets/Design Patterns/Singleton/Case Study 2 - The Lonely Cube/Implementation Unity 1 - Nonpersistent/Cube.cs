using Shared = SingletonPattern.Case2;

// + unique

namespace SingletonPattern.Case2.Unity1 {
  public class Cube : MonoBehaviourCube {
    public static Cube Instance { get; private set; }

    private void Awake() {
      if (Instance)
        Destroy(gameObject);
      else
        Instance = this;
    }
  }
}