using UnityEngine;
using System.Collections;

public class CameraRaycast : MonoBehaviour {

  private Camera cameraCompo; // Camera component attached to your main camera
  private GameObject currentObject; // Object looked by your main camera

  // Use this for initialization
  void Start() {

    cameraCompo = GetComponent<Camera>();

  }

  // Update is called once per frame
  void Update() {

    RaycastHit hit;
    Vector3 cameraCenter = cameraCompo.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, cameraCompo.nearClipPlane)); // Calculation fo the center of your main camera

    // Raycast launch for detecting the object looked by your camera
    // Object looked is saved into a variable
    // If another object than that saved is looked by camera, a function is called to simulate a "center exit" (as well as a MouseExit)
    // If no object is looked by camera, a function is called to simulate a "center exit" (as well as a MouseExit)
    if (Physics.Raycast(cameraCenter, this.transform.forward, out hit, 1000)) {
      if (currentObject != hit.collider.gameObject && currentObject != null)
        currentObject.SendMessage("lookedByCam_exit", SendMessageOptions.DontRequireReceiver);
      currentObject = hit.collider.transform.gameObject;
      currentObject.SendMessage("lookedByCam_enter", SendMessageOptions.DontRequireReceiver);
    } else {
      if (currentObject != null)
        currentObject.SendMessage("lookedByCam_exit", SendMessageOptions.DontRequireReceiver);
    }

  }
}
