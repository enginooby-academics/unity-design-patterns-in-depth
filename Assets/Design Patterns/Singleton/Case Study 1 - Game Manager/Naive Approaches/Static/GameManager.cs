// + eager initialization
// + global access
// + scene-persistent: no if access variable via static instance
// + unique: no
namespace Singleton.Static {
  public class GameManager : Singleton.GameManager {
    public static GameManager Instance;

    private void Awake() {
      Instance = this;
    }
  }
}