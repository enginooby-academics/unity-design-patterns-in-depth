using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostFxController : MonoBehaviour {
  private VolumeEditor volumeEditor;

  void Start() {
    volumeEditor = GameObject.FindObjectOfType<VolumeEditor>();
  }

  void Update() {
    if (InputUtils.MouseScrollUp()) volumeEditor.SwitchNextVolume();
    if (InputUtils.MouseScrollDown()) volumeEditor.SwitchPreviousVolume();
  }
}