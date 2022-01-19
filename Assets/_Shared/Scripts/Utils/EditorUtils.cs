namespace Enginoobz.Utils {
  public static class EditorUtils {
    public static void StopPlayMode() {
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
  }
}