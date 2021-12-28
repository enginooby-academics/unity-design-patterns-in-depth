using Shared = SingletonPattern.Case1;

// + eager init
// + scene-persistent: no
// + unique
namespace SingletonPattern.Case1.Unity1 {
  public class GameManager : Shared.GameManager {
    public static GameManager Instance;

    private void Awake() {
      if (Instance) {
        Destroy(gameObject);
      } else {
        Instance = this;
      }
    }
  }
}
