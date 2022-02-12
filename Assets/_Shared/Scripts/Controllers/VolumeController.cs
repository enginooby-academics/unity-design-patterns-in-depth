using UnityEngine;

public class VolumeController : MonoBehaviour {
  private VolumeEditor volumeEditor;

  private void Start() {
    volumeEditor = FindObjectOfType<VolumeEditor>();
  }

  private void Update() {
    if (Input.GetKeyUp(KeyCode.J)) volumeEditor.SwitchPreviousVolume();
    if (Input.GetKeyUp(KeyCode.K)) volumeEditor.SwitchNextVolume();
    if (Input.GetAxis("Mouse ScrollWheel") > 0f) volumeEditor.SwitchNextVolume();
    if (Input.GetAxis("Mouse ScrollWheel") < 0f) volumeEditor.SwitchPreviousVolume();
  }
}