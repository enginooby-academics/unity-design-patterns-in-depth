using UnityEngine;

namespace Enginoobz.Graphics {
  public class PostFxController : MonoBehaviour {
    private VolumeEditor volumeEditor;

    private void Start() {
      volumeEditor = FindObjectOfType<VolumeEditor>();
    }

    private void Update() {
      if (InputUtils.MouseScrollUp()) volumeEditor.SwitchNextVolume();
      if (InputUtils.MouseScrollDown()) volumeEditor.SwitchPreviousVolume();
    }
  }
}