using Shared = SingletonPattern.Case1;

// + eager init
// + scene-persistent: no if access variable via static instance
// + unique: no
namespace SingletonPattern.Case1.Naive1 {
  public class GameManager : Shared.GameManager {
    public static GameManager Instance;

    private void Awake() {
      Instance = this;
    }
  }
}