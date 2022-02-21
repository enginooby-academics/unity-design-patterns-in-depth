// * Use cases: for simple transformations (w/ its loop & constrain). E.g: bullet, non-physical controller
// * For complex movements, use path tools such as Simple Waypoint System

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginooby.Attribute;
#endif

using UnityEngine;
using static VectorUtils;

// TODO:
// + rotational speed/acceleration w/ max speed
// + random range for params (speed/acceleration)
// + optional loop (w/ time)
// + max offset (constrain) w/ optional loop
// + local vs. World-space
// + integrate DOTween
// + events

// CONSIDER:
// ? hotkey for enable/disable

public class TransformOperator : ComponentOperator<Transform> {
  [SerializeField] [HideLabel] private Translater _translater;

  #region SCALING

  // ===================================================================================================================
  [ToggleGroup(nameof(_enableScaling), "Scaling")] [SerializeField]
  private bool _enableScaling;

  #endregion

  protected override void Awake() {
    _translater.Setup(Transform);
    _translater.Init();
  }

  protected override void LateUpdate() {
    ProcessLooking();
    _translater.ProcessTranslating();
    ProcessRotating();
  }

  protected override void DrawGizmos() {
    _translater.DrawGizmosTranslating();
  }

  public void Stop() {
    StopLooking();
    _translater.StopTranslating();
    StopRotating();
  }

  private enum Mode {
    Auto,
    Control,
  }

  private enum OnBoundaryAction {
    Stop,
    LoopMinToMax,
    LoopMaxToMin,
    LoopPingPong,
  }

  #region LOOK AT

  // ===================================================================================================================

  [ToggleGroup(nameof(_enableLookAt), "Loot At")] [SerializeField]
  private bool _enableLookAt;

  // TODO: Damping

  [ToggleGroup(nameof(_enableLookAt))] [SerializeField] [LabelText("Target")]
  private Reference _lookAtTarget;

  private void ProcessLooking() {
    if (!_enableLookAt) return;

    // TODO: parameterize direction +-X/Y/Z
    // UTIL
    transform.forward = _lookAtTarget.GameObject.transform.forward;
  }

  public void StopLooking() {
    _enableLookAt = false;
  }

  #endregion


  #region ROTATING

  // ===================================================================================================================
  [ToggleGroup(nameof(_enableRotating), "Rotating")] [SerializeField]
  private bool _enableRotating;

  [ToggleGroup(nameof(_enableRotating))] [SerializeField] [LabelText("Speed")]
  private Vector3 _rotationalSpeed;

  [ToggleGroup(nameof(_enableRotating))] [SerializeField] [LabelText("Acceleration")]
  private Vector3 _rotationalAcceleration;

  [ToggleGroup(nameof(_enableRotating))]
  [InlineButton(nameof(SetSelfAsRotationPivot), "Self")]
  [SerializeField]
  [LabelText("Pivot")]
  private Transform _rotationPivot;

  [ToggleGroup(nameof(_enableRotating))] [SerializeField] [EnumToggleButtons]
  private Mode _rotationMode = Mode.Auto;

  [ToggleGroup(nameof(_enableRotating))] [ShowIf(nameof(_rotationMode), Mode.Control)] [SerializeField]
  private InputModifier _xRotateKey = new();

  [ToggleGroup(nameof(_enableRotating))] [ShowIf(nameof(_rotationMode), Mode.Control)] [SerializeField]
  private InputModifier _yRotateKey = new();

  [ToggleGroup(nameof(_enableRotating))] [ShowIf(nameof(_rotationMode), Mode.Control)] [SerializeField]
  private InputModifier _zRotateKey = new();

  private void SetSelfAsRotationPivot() {
    _rotationPivot = transform;
  }

  private void ProcessRotating() {
    if (!_enableRotating) return;

    if (_rotationPivot) ProcessRotatingWithPivot();
    else ProcessRotatingWithoutPivot();
  }

  private void ProcessRotatingWithoutPivot() {
    if (_rotationMode == Mode.Control) {
      Transform.RotateX(_rotationalSpeed.x * _xRotateKey.InputValue);
      Transform.RotateY(_rotationalSpeed.y * _yRotateKey.InputValue);
      Transform.RotateZ(_rotationalSpeed.z * _zRotateKey.InputValue);
    }
    else {
      Transform.Rotate(_rotationalSpeed);
    }
  }

  private void ProcessRotatingWithPivot() {
    var pivotPosition = _rotationPivot.position;
    var angleX = _rotationalSpeed.x * Time.deltaTime;
    var angleY = _rotationalSpeed.y * Time.deltaTime;
    var angleZ = _rotationalSpeed.z * Time.deltaTime;

    if (_rotationMode == Mode.Control) {
      angleX *= _xRotateKey.InputValue;
      angleY *= _yRotateKey.InputValue;
      angleZ *= _zRotateKey.InputValue;
    }

    Transform.RotateAround(pivotPosition, v100, angleX);
    Transform.RotateAround(pivotPosition, v010, angleY);
    Transform.RotateAround(pivotPosition, v001, angleZ);
  }

  private void StopRotating() => _enableRotating = false;

  #endregion

  #region PUBLIC METHODS

  // ===================================================================================================================
  public void SetPosition(Vector3 pos) { }

  public void SetRotation(Vector3 rot) { }

  public void SetScale(Vector3 scale) { }

  #endregion
}