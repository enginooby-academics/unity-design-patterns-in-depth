using UnityEditor;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

// TODO:
// + Area boundary for each movement
// + Local coord for horizontal/vertial movements (by using AddRelativeForce)

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyController : MonoBehaviourBase {
  private bool isOnGround = true;
  private Rigidbody rb;

  private void Start() {
    rb = this.AddRigidBodyIfNotExist();
    anim = GetComponent<Animator>();
    InitForceAndTorque();
    PlayRunAnimation();
    SetGravity(gravityPercent);
  }

  private void FixedUpdate() {
    if (GameManager.Instance.gameOver) return;
    if (enableJump) ProcessJump();
    if (enableVerticalMovement) ProcessVerticalMovement();
    if (enableHorizontalMovement) ProcessHorizontalMovement();
  }

  private void LateUpdate() {
    ConstraintMovements();
  }

  // TODO: Implement separate Collector/Collectable or Invertory system
  // ! Specific
  private void OnCollisionEnter(Collision other) {
    if (GameManager.Instance.gameOver) return;

    if (other.gameObject.CompareTag("Obstacle")) {
      Fall();
    }
    else {
      if (runningVfx) runningVfx.Play();
      isOnGround = true;
    }
  }

  private void OnDrawGizmos() {
    DrawGismozMovemenConstraints();
  }

  private void ConstraintMovements() {
    var pos = transform.position;

    if (enableJump && !jumpableHeightRange.ContainsIgnoreZero(pos.y)) transform.PosY(jumpableHeightRange.Clamp(pos.y));
    if (enableHorizontalMovement && !horizontalMovementRange.ContainsIgnoreZero(pos.x))
      transform.PosX(horizontalMovementRange.Clamp(pos.x)); // ! jiggering constraint
    if (enableVerticalMovement && !verticalMovementRange.ContainsIgnoreZero(pos.z))
      transform.PosZ(verticalMovementRange.Clamp(pos.z));
  }

  private void DrawGismozMovemenConstraints() {
    if (enableJump) {
      //
      var color = CanJump ? Color.magenta : Color.red;
      this.DrawGizmozRangeFromPos(jumpableHeightRange, Axis.Y, color);
    }

    if (enableHorizontalMovement) {
      var color = CanMoveHorizontal ? Color.magenta : Color.red;
      this.DrawGizmozRangeFromPos(horizontalMovementRange, Axis.X, color);
    }

    if (enableVerticalMovement) {
      var color = CanMoveVertical ? Color.magenta : Color.red;
      this.DrawGizmozRangeFromPos(verticalMovementRange, Axis.Z, color);
    }
  }

  // !  Specific
  private void Fall() {
    PlayFallAnimation();
    if (fallVfx) fallVfx.Play();
    if (crashSfx) GameManager.Instance.audioSource.PlayOneShot(crashSfx);
    if (runningVfx) runningVfx.Stop();
  }

  #region PHYSICS ===================================================================================================================================

  // ? Separate into PhysicsController
  [SerializeField] [ToggleLeft] private bool enableGravityChange;

  [EnableIf(nameof(enableGravityChange))]
  [OnValueChanged(nameof(SetGravity))]
  [SerializeField]
  [Range(1, 500)]
  [SuffixLabel("%")]
  [LabelText("Gravity")]
  private float gravityPercent = 100;

  private void SetGravity(float percent) {
#if UNITY_EDITOR
    if (!EditorApplication.isPlaying) return;
#endif
    if (!enableGravityChange) return;
    gravityPercent = percent;
    Physics.gravity = new Vector3(0, -9.8f * gravityPercent / 100f, 0);
  }

  #endregion ===================================================================================================================================

  #region JUMP ===================================================================================================================================

  [ToggleGroup(nameof(enableJump), "JUMP")] [SerializeField]
  private bool enableJump = true;

  [ToggleGroup(nameof(enableJump))] [SerializeField]
  private InputModifier jumpKey = new InputModifier(keyCode: KeyCode.Space);

  [ToggleGroup(nameof(enableJump))] [SerializeField]
  private float jumpForce = 10f;

  [ToggleGroup(nameof(enableJump))] [SerializeField] [EnumToggleButtons]
  private ForceMode jumpForceMode = ForceMode.Impulse;

  // TODO: bool limitJumpTimes
  [ToggleGroup(nameof(enableJump))] [SerializeField] [Range(1, 5)]
  private int jumpMaxTimes = 1;

  [ToggleGroup(nameof(enableJump))] [SerializeField] [LabelText("Checking ground")]
  private bool checkingGroundForJump;

  [ToggleGroup(nameof(enableJump))]
  [MinMaxSlider(nameof(dynamicMinMax), true)]
  [InfoBox("Default (0, 0) is not limit, player can jump at any height")]
  [SerializeField]
  public Vector2 jumpableHeightRange = Vector2.zero;

  [ToggleGroup(nameof(enableJump))] [SerializeField] [LabelText("Min Max")]
  public Vector2 dynamicMinMax = new Vector2(0, 20);

  private int jumpCount;

  private bool CanJump =>
    jumpableHeightRange.ContainsIgnoreZero(transform.position.y) &&
    jumpKey.IsTriggering &&
    (checkingGroundForJump && isOnGround || !checkingGroundForJump);

  private void ProcessJump() {
    if (!CanJump) return;

    PlayJumpAnimation();
    if (jumpSfx) GameManager.Instance.audioSource.PlayOneShot(jumpSfx);
    if (runningVfx) runningVfx.Stop();
    rb.AddForce(Vector3.up * jumpForce, jumpForceMode);
    jumpCount++;
    isOnGround = false;
  }

  #endregion ===================================================================================================================================

  #region VERTICAL MOVE ===================================================================================================================================

  [ToggleGroup(nameof(enableVerticalMovement), "VERTICAL")] [SerializeField]
  private bool enableVerticalMovement = true; // TODO: disable Freeze Position Z on Rigidbody

  [ToggleGroup(nameof(enableVerticalMovement))] [SerializeField]
  private InputModifier verticalKey = new InputModifier(InputModifier.InputType.Axis, inputAxis: InputAxis.Vertical);

  [ToggleGroup(nameof(enableVerticalMovement))] [SerializeField]
  private float verticalSpeed = 5f;

  [ToggleGroup(nameof(enableVerticalMovement))] [SerializeField] [EnumToggleButtons] [LabelText("Force Mode")]
  private ForceMode verticalForceMode = ForceMode.Impulse;

  [ToggleGroup(nameof(enableVerticalMovement))] [SerializeField] [LabelText("Checking ground")]
  private bool checkingGroundForVertical = true;

  [ToggleGroup(nameof(enableVerticalMovement))]
  [MinMaxSlider(nameof(dynamicMinMaxForVerticalRange), true)]
  [InfoBox("Default (0, 0) is not limit, player can move at any point")]
  [SerializeField]
  public Vector2 verticalMovementRange = Vector2.zero;

  [ToggleGroup(nameof(enableVerticalMovement))] [SerializeField] [LabelText("Min Max")]
  public Vector2 dynamicMinMaxForVerticalRange = new Vector2(-20, 20);

  [ToggleGroup(nameof(enableVerticalMovement))] [SerializeField] [LabelText("Enable Boost")]
  private bool enableVerticalBoost;

  [ToggleGroup(nameof(enableVerticalMovement))]
  [ShowIf(nameof(enableVerticalBoost))]
  [SerializeField]
  [LabelText("Boost Speed")]
  private float verticalBoostSpeed = 2f;

  [ToggleGroup(nameof(enableVerticalMovement))]
  [ShowIf(nameof(enableVerticalBoost))]
  [SerializeField]
  [LabelText("Boost VFX")]
  private ParticleSystem? verticalBoostVfx;

  [ToggleGroup(nameof(enableVerticalMovement))]
  [ShowIf(nameof(enableVerticalBoost))]
  [SerializeField]
  [LabelText("Boost SFX")]
  private AudioClip? verticalBoostSfx;

  [ToggleGroup(nameof(enableVerticalMovement))]
  [ShowIf(nameof(enableVerticalBoost))]
  [SerializeField]
  [LabelText("Boost Key")]
  private InputModifier verticalBoostKey = new InputModifier(InputModifier.InputType.KeyCode,
    keyTriggerEvent: KeyCodeTriggerEvent.Hold, keyCode: KeyCode.Space);


  private bool CanMoveVertical =>
    verticalMovementRange.ContainsIgnoreZero(transform.position
      .z) && // ? Can remove, since already constraint in FixUpdate()
    (checkingGroundForVertical && isOnGround || !checkingGroundForVertical);

  private void ProcessVerticalMovement() {
    if (!CanMoveVertical) return;
    if (verticalKey.InputValue == 0) return; // for performance

    var verticalDirection = enableFocalPoint ? focalPoint.transform.forward : Vector3.forward;
    var totalVerticalSpeed = verticalSpeed + GetVerticalBoostSpeed();
    rb.AddForce(verticalDirection * totalVerticalSpeed * verticalKey.InputValue, verticalForceMode);
  }

  private float GetVerticalBoostSpeed() {
    if (!enableVerticalBoost) return 0;

    if (!verticalBoostKey.IsTriggering) {
      if (verticalBoostVfx) verticalBoostVfx.Stop();
      return 0;
    }

    if (verticalBoostVfx) verticalBoostVfx.Play();
    if (verticalBoostSfx) verticalBoostSfx.PlayOneShot();
    return verticalBoostSpeed;
  }

  #endregion ===================================================================================================================================

  #region HORIZONTAL MOVE ===================================================================================================================================

  [ToggleGroup(nameof(enableHorizontalMovement), "HORIZONTAL")] [SerializeField]
  private bool enableHorizontalMovement = true; // TODO: disable Freeze Position X on Rigidbody

  [ToggleGroup(nameof(enableHorizontalMovement))] [SerializeField]
  private InputModifier horizontalKey =
    new InputModifier(InputModifier.InputType.Axis, inputAxis: InputAxis.Horizontal);

  [ToggleGroup(nameof(enableHorizontalMovement))] [SerializeField]
  private float horizontalSpeed = 5f;

  [ToggleGroup(nameof(enableHorizontalMovement))] [SerializeField] [EnumToggleButtons] [LabelText("Force Mode")]
  private ForceMode horizontalForceMode = ForceMode.Impulse;

  [ToggleGroup(nameof(enableHorizontalMovement))] [SerializeField] [LabelText("Checking ground")]
  private bool checkingGroundForHorizontal = true;

  [ToggleGroup(nameof(enableHorizontalMovement))]
  [MinMaxSlider(nameof(dynamicMinMaxForHorizontalRange), true)]
  [InfoBox("Default (0, 0) is not limit, player can move at any point")]
  [SerializeField]
  public Vector2 horizontalMovementRange = Vector2.zero;

  [ToggleGroup(nameof(enableHorizontalMovement))] [SerializeField] [LabelText("Min Max")]
  public Vector2 dynamicMinMaxForHorizontalRange = new Vector2(-20, 20);

  private bool CanMoveHorizontal =>
    horizontalMovementRange.ContainsIgnoreZero(transform.position
      .x) && // ? Can remove, since already constraint in FixUpdate()
    (checkingGroundForHorizontal && isOnGround || !checkingGroundForHorizontal);

  private void ProcessHorizontalMovement() {
    if (!CanMoveHorizontal) return;
    if (horizontalKey.InputValue == 0) return;

    var horizontalDirection = enableFocalPoint ? focalPoint.transform.right : Vector3.right;
    rb.AddForce(horizontalDirection * horizontalSpeed * horizontalKey.InputValue, horizontalForceMode);
  }

  #endregion ===================================================================================================================================

  #region FOCAL POINT ===================================================================================================================================

  // TODO: InfoBox
  [ToggleGroup(nameof(enableFocalPoint), "FOCAL POINT")] [SerializeField]
  private bool enableFocalPoint;

  [ToggleGroup(nameof(enableFocalPoint))] [SerializeField]
  private Transform focalPoint;

  #endregion ===================================================================================================================================

  #region INITIAL FORCE - forces applied on gameoject at the beginning ===================================================================================================================================

  // TODO: InfoBox
  [ToggleGroup(nameof(enableInitialForce), "INITAL FORCE")] [SerializeField]
  private bool enableInitialForce;

  [ToggleGroup(nameof(enableInitialForce))] [SerializeField] [EnumToggleButtons] [LabelText("Force Axis")]
  private AxisFlag initialForceAxis = AxisFlag.Y;

  [ToggleGroup(nameof(enableInitialForce))] [SerializeField] [LabelText("Force Mode")] [EnumToggleButtons]
  private ForceMode initialForceMode = ForceMode.Impulse;

  [ToggleGroup(nameof(enableInitialForce))] [SerializeField] [MinMaxSlider(-20, 20, true)] [LabelText("Force Range")]
  private Vector2 initalForceRange = new Vector2(10, 12);

  [ToggleGroup(nameof(enableInitialForce))] [SerializeField] [LabelText("Torque Mode")] [EnumToggleButtons]
  private ForceMode initialTorqueMode = ForceMode.Impulse;

  [ToggleGroup(nameof(enableInitialForce))]
  // TODO: create Composite Attribute for Serializable: 
  // [HideLabel, Title("Torque Range", bold: false), PropertySpace(-8)]
  [HideLabel]
  // [LabelText("Torque Range")]
  [SerializeField]
  private Vector3Range initalTorqueRange = new Vector3Range("Torque Range");

  private void InitForceAndTorque() {
    if (!enableInitialForce) return;

    rb.AddForce(initialForceAxis.ToVector3() * initalForceRange.Random(), initialForceMode);
    rb.AddTorque(initalTorqueRange.Random, initialTorqueMode);
  }

  #endregion ===================================================================================================================================

  // TODO: Seperate FXs & Animation into different ComponentManipulor

  #region FX ===================================================================================================================================

  [BoxGroup("FXs")] [SerializeField] [LabelText("Fall VFX")]
  private ParticleSystem fallVfx;

  [BoxGroup("FXs")] [SerializeField] [LabelText("Running VFX")]
  private ParticleSystem runningVfx;

  [BoxGroup("FXs")] [SerializeField] [LabelText("Jump SFX")]
  private AudioClip jumpSfx;

  [BoxGroup("FXs")] [SerializeField] [LabelText("Crash SFX")]
  private AudioClip crashSfx;

  #endregion ===================================================================================================================================

  #region ANIMATION ===================================================================================================================================

  private Animator anim;

  // !Specific
  private void PlayRunAnimation() {
    if (!anim) return;
    anim.SetBool("Static_b", true);
    anim.SetFloat("Speed_f", 0.6f);
  }

  private void PlayJumpAnimation() {
    if (!anim) return;
    anim.SetTrigger("Jump_trig");
  }

  private void PlayFallAnimation() {
    if (!anim) return;
    anim.SetBool("Death_b", true);
    anim.SetInteger("DeathType_int", 1);
  }

  #endregion ===================================================================================================================================

  #region EVENTS & PUBLIC METHODS - customize for specific project ===================================================================================================================================

  [ToggleGroup(nameof(enableEvent), "EVENT")] [SerializeField]
  private bool enableEvent;

  public void ResetVelocity() {
    rb.velocity = Vector3.zero;
    rb.angularVelocity = Vector3.zero;
  }

  #endregion
}