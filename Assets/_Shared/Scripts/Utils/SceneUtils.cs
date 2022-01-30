using UnityEngine.SceneManagement;
using UnityEngine;

public static class SceneUtils {
  /// <summary>
  /// Reload the active scene. No need adding scenen to Build setting.
  /// </summary>
  public static void ReloadScene() {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  public static void ReloadSceneOnKeyDown(KeyCode triggerKey) {
    if (triggerKey.IsDown()) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
}