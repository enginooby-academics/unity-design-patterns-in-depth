#if ASSET_REWIRED
/* Use cases: fly camera

[wasdqe] : basic movement
hold [shift] : Accelerate speed
hold [space] : Lock movement on Y axis (don't change height) */

using UnityEngine;
using Rewired;

public class RewiredFlyingMovementController : MonoBehaviour {
  public int playerId = 0; // The Rewired player id of this controller
  private Player player;  //The Rewired player
  public float mainSpeed = 5.0f; //regular speed
  public float accelerateIncrement = 10.0f; //multiplied by how long acclerate key is held.  Basically running
  public float maxSpeed = 100.0f; //Maximum speed when accelerating
  public float rotateSensitivity = 0.25f; //How sensitive it with mouse
  public bool rotateOnMouseDown = true;

  private Vector3 lastMouse;
  private float totalRun = 1.0f;

  //input stats
  private Vector3 moveVector; //{AWSDQE} - Horizontal, vertical, zedical
  private bool accelerated;   //{Shift}
  private bool movementStaysFlat; //{Space} - Movement on x & z axises only

  void Awake() {
    // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
    player = ReInput.players.GetPlayer(playerId);
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
    if (MouseButton.Right.IsDown()) {
      lastMouse = Input.mousePosition; // $CTK reset when we begin
    }

    if (!rotateOnMouseDown || (rotateOnMouseDown && MouseButton.Right.IsHeld())) {
      lastMouse = Input.mousePosition - lastMouse;
      lastMouse = new Vector3(-lastMouse.y * rotateSensitivity, lastMouse.x * rotateSensitivity, 0);
      lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
      transform.eulerAngles = lastMouse;
      lastMouse = Input.mousePosition;
    }
  }

  // REFACTOR: Replace string by enums
  private void GetInput() {
    moveVector +=
 new Vector3(player.GetAxis("Move Horizontal"), player.GetAxis("Move Vertical"), player.GetAxis("Move Zedical"));
    accelerated = player.GetButton("Accelerate");
    movementStaysFlat = player.GetButton("Stay Flat");
  }
}
#endif