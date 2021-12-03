// * Use cases: for simple transformations (w/ its loop & constrain). E.g: bullet, non-physical controller
// * For complex movements, use path tools such as Simple Waypoint System
using UnityEngine;
using Sirenix.OdinInspector;
using static VectorUtils;

// TODO:
// + rotational speed/acceleration w/ max speed
// + random range for params (speed/acceleration)
// + optional loop (w/ time)
// + max offset (constrain) w/ optional loop
// + local vs. World-space
// + integrate DOTween

// ? CONSIDER:
// ? hotkey for enable/disable

public class TransformOperator : MonoBehaviourBase {
  enum Mode { Auto, Control }

  private void Start() {
  }

  void LateUpdate() {
    ProcessLookAt();
    ProcessTranslating();
    ProcessRotating();
  }

  public void Stop() {
    StopLookAt();
    StopTranslating();
    StopRotating();
  }

  void OnDrawGizmosSelected() {
    DrawGizmosTranslating();
  }

  #region LOOK AT ===================================================================================================================================
  [ToggleGroup(nameof(_enableLookAt), groupTitle: "Loot At")]
  [SerializeField]
  private bool _enableLookAt;

  // TODO: Damping

  [ToggleGroup(nameof(_enableLookAt))]
  [SerializeField, LabelText("Target")]
  private Reference _lookAtTarget;

  private void ProcessLookAt() {
    if (!_enableLookAt) return;

    // TODO: parameterize direction +-X/Y/Z
    // UTIL
    transform.forward = _lookAtTarget.GameObject.transform.forward;
  }

  public void StopLookAt() {
    _enableLookAt = false;
  }
  #endregion ===================================================================================================================================

  #region TRANSLATING ===================================================================================================================================
  [ToggleGroup(nameof(_enableTranslating), groupTitle: "Translating")]
  [SerializeField]
  private bool _enableTranslating = true;

  [ToggleGroup(nameof(_enableTranslating))]
  [SerializeField, LabelText("Speed")]
  private Vector3 _translationalSpeed;

  public Vector3 TranslationalSpeed {
    get => _translationalSpeed;
    set => _translationalSpeed = value;
  }

  [ToggleGroup(nameof(_enableTranslating))]
  [SerializeField, LabelText("Acceleration")]
  private Vector3 _translationalAcceleration;

  [ToggleGroup(nameof(_enableTranslating))]
  [SerializeField, LabelText("Offset Bound")]
  private Vector3 _translationalOffsetBound;

  [ToggleGroup(nameof(_enableTranslating))]
  [SerializeField, EnumToggleButtons]
  private Mode _translatingMode = Mode.Auto;

  [ToggleGroup(nameof(_enableTranslating))]
  [ShowIf(nameof(_translatingMode), Mode.Control)]
  [SerializeField]
  private InputModifier _xTranslateKey = new InputModifier(inputType: InputModifier.InputType.Axis);

  [ToggleGroup(nameof(_enableTranslating))]
  [ShowIf(nameof(_translatingMode), Mode.Control)]
  [SerializeField]
  private InputModifier _yTranslateKey = new InputModifier(inputType: InputModifier.InputType.Axis);

  [ToggleGroup(nameof(_enableTranslating))]
  [ShowIf(nameof(_translatingMode), Mode.Control)]
  [SerializeField]
  private InputModifier _zTranslateKey = new InputModifier(inputType: InputModifier.InputType.Axis, inputAxis: InputAxis.Horizontal);

  void DrawGizmosTranslating() {
    if (!_enableTranslating) return;

    gameObject.DrawGizmosDirection(_translationalSpeed);
    Gizmos.DrawSphere(gameObject.transform.position + _translationalOffsetBound, .2f);
  }

  void ProcessTranslating() {
    if (!_enableTranslating) return;

    if (_translatingMode == Mode.Auto) {
      this.MoveWorld(distances: _translationalSpeed);
    } else {
      this.MoveXWorld(distance: _xTranslateKey.InputValue * _translationalSpeed.x);
      this.MoveYWorld(distance: _yTranslateKey.InputValue * _translationalSpeed.y);
      this.MoveZWorld(distance: _zTranslateKey.InputValue * _translationalSpeed.z);
    }
  }

  void StopTranslating() {
    _enableTranslating = false;
  }
  #endregion ===================================================================================================================================

  #region ROTATING ===================================================================================================================================
  [ToggleGroup(nameof(_enableRotating), groupTitle: "Rotating")]
  [SerializeField]
  private bool _enableRotating;

  [ToggleGroup(nameof(_enableRotating))]
  [SerializeField, LabelText("Speed")]
  private Vector3 _rotationalSpeed;

  [ToggleGroup(nameof(_enableRotating))]
  [SerializeField, LabelText("Acceleration")]
  Vector3 rotationalAcceleration;

  [ToggleGroup(nameof(_enableRotating))]
  [InlineButton(nameof(SetSelfAsRotationPivot), "Self")]
  [SerializeField, LabelText("Pivot")]
  private Transform _rotationPivot;

  private void SetSelfAsRotationPivot() {
    _rotationPivot = transform;
  }

  [ToggleGroup(nameof(_enableRotating))]
  [SerializeField, EnumToggleButtons]
  private Mode _rotationMode = Mode.Auto;

  [ToggleGroup(nameof(_enableRotating))]
  [ShowIf(nameof(_rotationMode), Mode.Control)]
  [SerializeField]
  private InputModifier _xRotateKey = new InputModifier();

  [ToggleGroup(nameof(_enableRotating))]
  [ShowIf(nameof(_rotationMode), Mode.Control)]
  [SerializeField]
  private InputModifier _yRotateKey = new InputModifier();

  [ToggleGroup(nameof(_enableRotating))]
  [ShowIf(nameof(_rotationMode), Mode.Control)]
  [SerializeField]
  private InputModifier _zRotateKey = new InputModifier();

  void ProcessRotating() {
    if (!_enableRotating) return;

    if (_rotationPivot) ProcessRotatingWithPivot();
    else ProcessRotatingWithoutPivot();
  }

  void ProcessRotatingWithoutPivot() {
    if (_rotationMode == Mode.Control) {
      this.RotateX(_rotationalSpeed.x * _xRotateKey.InputValue);
      this.RotateY(_rotationalSpeed.y * _yRotateKey.InputValue);
      this.RotateZ(_rotationalSpeed.z * _zRotateKey.InputValue);
    } else {
      this.Rotate(angles: _rotationalSpeed);
    }
  }

  void ProcessRotatingWithPivot() {
    if (_rotationMode == Mode.Control) {
      transform.RotateAround(_rotationPivot.position, v100, _rotationalSpeed.x * Time.deltaTime * _xRotateKey.InputValue);
      transform.RotateAround(_rotationPivot.position, v010, _rotationalSpeed.y * Time.deltaTime * _yRotateKey.InputValue);
      transform.RotateAround(_rotationPivot.position, v001, _rotationalSpeed.z * Time.deltaTime * _zRotateKey.InputValue);
    } else {
      transform.RotateAround(_rotationPivot.position, v100, _rotationalSpeed.x * Time.deltaTime);
      transform.RotateAround(_rotationPivot.position, v010, _rotationalSpeed.y * Time.deltaTime);
      transform.RotateAround(_rotationPivot.position, v001, _rotationalSpeed.z * Time.deltaTime);
    }
  }

  void StopRotating() {
    _enableRotating = false;
  }
  #endregion ===================================================================================================================================

  #region SCALING ===================================================================================================================================
  [ToggleGroup(nameof(_enableScaling), groupTitle: "Scaling")]
  [SerializeField]
  private bool _enableScaling;

  #endregion ===================================================================================================================================


  #region PUBLIC METHODS ===================================================================================================================================
  public void SetPosition(Vector3 pos) {

  }

  public void SetRotation(Vector3 rot) {

  }

  public void SetScale(Vector3 scale) {

  }
  #endregion ===================================================================================================================================

}
