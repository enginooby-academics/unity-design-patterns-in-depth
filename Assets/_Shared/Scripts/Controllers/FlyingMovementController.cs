/* Use cases: fly camera

[wasdqe] : basic movement
hold [shift] : Accelerate speed
hold [space] : Lock movement on Y axis (don't change height) */

using UnityEngine;

public class FlyingMovementController : MonoBehaviour {
  public float mainSpeed = 10f; //regular speed
  public float accelerateIncrement = 20f; // multiplied by how long acclerate key is held. Basically running
  public float maxSpeed = 100f; // maximum speed when accelerating
  public float rotateSensitivity = 0.25f; // how sensitive it with mouse
  public bool rotateOnMouseDown = true;

  private Rigidbody m_Rigidbody;

  private Vector3 lastMouse;
  private float totalRun = 1.0f;

  //input stats
  private Vector3 moveVector; //{AWSDQE} - Horizontal, vertical, QE thing
  private bool accelerated;   //{Shift}
  private bool movementStaysFlat; //{Space} - Movement on x & z axises only

  void Awake() {
    // ResetCamera();
  }

  void ResetCamera() {
    Debug.Log("FlyCamera Awake() - RESETTING CAMERA POSITION");
    transform.position = new Vector3(0, 8, -32);
    transform.rotation = Quaternion.Euler(25, 0, 0);
  }

  void Update() {
    GetInput();
    ProcessInput();
  }

  private void ProcessInput() {
    ProcessRotationInput();
    ProcessAccelerationInput();
    ProcessMovementInput();
  }

  private void ProcessMovementInput() {
    moveVector = moveVector * Time.deltaTime;

    if (movementStaysFlat) {
      Vector3 newPosition = transform.position;
      transform.Translate(moveVector);
      newPosition.x = transform.position.x;
      newPosition.z = transform.position.z;
      transform.position = newPosition;
    } else {
      transform.Translate(moveVector);
    }
  }

  private void ProcessAccelerationInput() {
    if (accelerated) {
      totalRun += Time.deltaTime;
      moveVector = moveVector * totalRun * accelerateIncrement;
      moveVector.x = Mathf.Clamp(moveVector.x, -maxSpeed, maxSpeed);
      moveVector.y = Mathf.Clamp(moveVector.y, -maxSpeed, maxSpeed);
      moveVector.z = Mathf.Clamp(moveVector.z, -maxSpeed, maxSpeed);
    } else {
      totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
      moveVector = moveVector * mainSpeed;
    }
  }

  private void ProcessRotationInput() {
    if (Input.GetMouseButtonDown(1)) {
      lastMouse = Input.mousePosition; // $CTK reset when we begin
    }

    if (!rotateOnMouseDown || (rotateOnMouseDown && Input.GetMouseButton(1))) {
      lastMouse = Input.mousePosition - lastMouse;
      lastMouse = new Vector3(-lastMouse.y * rotateSensitivity, lastMouse.x * rotateSensitivity, 0);
      lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
      transform.eulerAngles = lastMouse;
      lastMouse = Input.mousePosition;
    }
  }

  // TODO: use Cross Platform Input or Input System
  private void GetInput() {
    if (Input.GetKey(KeyCode.W)) {
      moveVector += new Vector3(0, 0, 1);
    }
    if (Input.GetKey(KeyCode.S)) {
      moveVector += new Vector3(0, 0, -1);
    }
    if (Input.GetKey(KeyCode.A)) {
      moveVector += new Vector3(-1, 0, 0);
    }
    if (Input.GetKey(KeyCode.D)) {
      moveVector += new Vector3(1, 0, 0);
    }
    if (Input.GetKey(KeyCode.Q)) {
      moveVector += new Vector3(0, -1, 0);
    }
    if (Input.GetKey(KeyCode.E)) {
      moveVector += new Vector3(0, 1, 0);
    }

    accelerated = Input.GetKey(KeyCode.LeftShift);
    movementStaysFlat = Input.GetKey(KeyCode.Space);
  }
}