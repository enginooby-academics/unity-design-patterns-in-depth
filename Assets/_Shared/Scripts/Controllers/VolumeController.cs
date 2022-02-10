using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour {
  private VolumeEditor volumeEditor;

  void Start() {
    volumeEditor = GameObject.FindObjectOfType<VolumeEditor>();
  }

  void Update() {
    if (Input.GetKeyUp(KeyCode.J)) volumeEditor.SwitchPreviousVolume();
    if (Input.GetKeyUp(KeyCode.K)) volumeEditor.SwitchNextVolume();
    if (Input.GetAxis("Mouse ScrollWheel") > 0f) volumeEditor.SwitchNextVolume();
    if (Input.GetAxis("Mouse ScrollWheel") < 0f) volumeEditor.SwitchPreviousVolume();
  }
}