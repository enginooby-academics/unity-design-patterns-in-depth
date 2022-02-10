using Shared = SingletonPattern.Case1;

// + eager init
// + scene-persistent
// + unique
namespace SingletonPattern.Case1.Unity2 {
  public class GameManager : Shared.GameManager {
    public static GameManager Instance;

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
