// * Use cases: for simple transformations (w/ its loop & constrain). E.g: bullet, non-physical controller
// * For complex movements, use path tools such as Simple Waypoint System

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginooby.Attribute;
#endif

using UnityEngine;
using static VectorUtils;

// ? TransformProfile (SO) + Operator Vs. TransformOperator

[CreateAssetMenu(fileName = "New Transform Profile", menuName = "Profile/Transform", order = 0)]
public class TransformProfile : ScriptableObject {
  #region SCALING ===================================================================================================================================

  [ToggleGroup(nameof(_enableScaling), "Scaling")] [SerializeField]
  private bool _enableScaling; // ! non-shared data

  #endregion ===================================================================================================================================

  private Vector3 _originalPos; // ! non-shared data (should move to MonoBehaviour)

  // public void Start(Transform transform) {
  //   InitTranslation(transform);
  // }

  public void OnLateUpdate(Transform transform) {
    ProcessLooking(transform);
    ProcessTranslating(transform);
    ProcessRotating(transform);
  }

  public void Stop() {
    // ! non-shared
    StopLooking();
    StopTranslating();
    StopRotating();
  }

  public void OnDrawGizmosSelected(GameObject gameObject) {
    DrawGizmosTranslating(gameObject);
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

  #region LOOK AT ===================================================================================================================================

  [ToggleGroup(nameof(_enableLookAt), "Loot At")] [SerializeField]
  private bool _enableLookAt; // ! non-shared data

  // TODO: Damping

  [ToggleGroup(nameof(_enableLookAt))] [SerializeField] [LabelText("Target")]
  private Reference _lookAtTarget; // ! non-shared data

  private void ProcessLooking(Transform transform) {
    if (!_enableLookAt) return;

    // TODO: parameterize direction +-X/Y/Z
    // UTIL
    transform.forward = _lookAtTarget.GameObject.transform.forward;
  }

  public void StopLooking() {
    _enableLookAt = false;
  }

  #endregion ===================================================================================================================================

  #region TRANSLATING ===================================================================================================================================

  [ToggleGroup(nameof(_enableTranslating), "Translating")] [SerializeField]
  private bool _enableTranslating = true; // ! non-shared data

  [ToggleGroup(nameof(_enableTranslating))]
  [OnValueChanged(nameof(InitTranslation))]
  [SerializeField]
  [LabelText("Speed")]
  private Vector3 _translationalSpeed;

  public Vector3 TranslationalSpeed {
    get => _translationalSpeed;
    set => _translationalSpeed = value;
  }

  [ToggleGroup(nameof(_enableTranslating))]
  [ShowIf(nameof(_translatingMode), Mode.Auto)]
  [OnValueChanged(nameof(InitTranslation))]
  [SerializeField]
  [LabelText("Min Position")]
  private Vector3 _translationalMinPos;

  [ToggleGroup(nameof(_enableTranslating))]
  [ShowIf(nameof(_translatingMode), Mode.Auto)]
  [OnValueChanged(nameof(InitTranslation))]
  [SerializeField]
  [LabelText("Max Position")]
  private Vector3 _translationalMaxPos;

  [ToggleGroup(nameof(_enableTranslating))]
  [ShowIf(nameof(_translatingMode), Mode.Auto)]
  [SerializeField]
  [LabelText("On Reach Boundary")]
  private OnBoundaryAction _translationalOnBoundaryAction = OnBoundaryAction.LoopPingPong;

  private bool IsTranslatingOnY => _translationalSpeed.y != 0;
  private bool? IsTranslatingToMaxY;

  public void InvertTranslationalY() {
    IsTranslatingToMaxY = !IsTranslatingToMaxY;
  }

  public void InvertTranslationalDirection() {
    InvertTranslationalY();
  }

  [ToggleGroup(nameof(_enableTranslating))] [SerializeField] [LabelText("Acceleration")]
  private Vector3 _translationalAcceleration;

  [ToggleGroup(nameof(_enableTranslating))] [SerializeField] [EnumToggleButtons]
  private Mode _translatingMode = Mode.Auto;

  [ToggleGroup(nameof(_enableTranslating))] [ShowIf(nameof(_translatingMode), Mode.Control)] [SerializeField]
  private InputModifier _xTranslateKey = new(InputModifier.InputType.Axis);

  [ToggleGroup(nameof(_enableTranslating))] [ShowIf(nameof(_translatingMode), Mode.Control)] [SerializeField]
  private InputModifier _yTranslateKey = new(InputModifier.InputType.Axis);

  [ToggleGroup(nameof(_enableTranslating))] [ShowIf(nameof(_translatingMode), Mode.Control)] [SerializeField]
  private InputModifier _zTranslateKey = new(InputModifier.InputType.Axis, inputAxis: InputAxis.Horizontal);

  public void InitTranslation(Transform transform) {
    _originalPos = transform.position;
    IsTranslatingToMaxY = _translationalSpeed.y > 0;
  }

  private void DrawGizmosTranslating(GameObject gameObject) {
    if (!_enableTranslating) return;

    gameObject.DrawGizmosDirection(_translationalSpeed);
    if (IsTranslatingOnY) {
      Gizmos.DrawSphere(_originalPos.WithY(_translationalMinPos.y), .2f);
      Gizmos.DrawSphere(_originalPos.WithY(_translationalMaxPos.y), .2f);
    }
  }

  private void ProcessTranslating(Transform transform) {
    if (!_enableTranslating) return;

    if (_translatingMode == Mode.Auto) {
      ProcessAutoTranslating(transform);
    }
    else {
      transform.MoveXWorld(_xTranslateKey.InputValue * _translationalSpeed.x);
      transform.MoveYWorld(_yTranslateKey.InputValue * _translationalSpeed.y);
      transform.MoveZWorld(_zTranslateKey.InputValue * _translationalSpeed.z);
    }
  }

  private void ProcessAutoTranslating(Transform transform) {
    if (IsTranslatingOnY) ProcessTranslationalBoundaryY(transform);
    transform.MoveWorld(_translationalSpeed);
  }

  private void ProcessTranslationalBoundaryY(Transform transform) {
    IsTranslatingToMaxY = transform.ReachingYMinOrMax(_translationalMinPos.y, _translationalMaxPos.y) ??
                          IsTranslatingToMaxY;
    switch (_translationalOnBoundaryAction) {
      case OnBoundaryAction.LoopPingPong:
        _translationalSpeed = IsTranslatingToMaxY.Value
          ? _translationalSpeed.WithPositiveY()
          : _translationalSpeed.WithNegativeY();
        break;
    }
  }

  private void StopTranslating() {
    _enableTranslating = false;
  }

  #endregion ===================================================================================================================================

  #region ROTATING ===================================================================================================================================

  [ToggleGroup(nameof(_enableRotating), "Rotating")] [SerializeField]
  private bool _enableRotating; // ! non-shared data

  [ToggleGroup(nameof(_enableRotating))] [SerializeField] [LabelText("Speed")]
  private Vector3 _rotationalSpeed;

  [ToggleGroup(nameof(_enableRotating))] [SerializeField] [LabelText("Acceleration")]
  private Vector3 _rotationalAcceleration;

  [ToggleGroup(nameof(_enableRotating))]
  // [InlineButton(nameof(SetSelfAsRotationPivot), "Self")]
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

  // private void SetSelfAsRotationPivot() => _rotationPivot = transform;

  private void ProcessRotating(Transform transform) {
    if (!_enableRotating) return;

    if (_rotationPivot) ProcessRotatingWithPivot(transform);
    else ProcessRotatingWithoutPivot(transform);
  }

  private void ProcessRotatingWithoutPivot(Transform transform) {
    if (_rotationMode == Mode.Control) {
      transform.RotateX(_rotationalSpeed.x * _xRotateKey.InputValue);
      transform.RotateY(_rotationalSpeed.y * _yRotateKey.InputValue);
      transform.RotateZ(_rotationalSpeed.z * _zRotateKey.InputValue);
    }
    else {
      transform.Rotate(angles: _rotationalSpeed);
    }
  }

  private void ProcessRotatingWithPivot(Transform transform) {
    if (_rotationMode == Mode.Control) {
      transform.RotateAround(_rotationPivot.position, v100,
        _rotationalSpeed.x * Time.deltaTime * _xRotateKey.InputValue);
      transform.RotateAround(_rotationPivot.position, v010,
        _rotationalSpeed.y * Time.deltaTime * _yRotateKey.InputValue);
      transform.RotateAround(_rotationPivot.position, v001,
        _rotationalSpeed.z * Time.deltaTime * _zRotateKey.InputValue);
    }
    else {
      transform.RotateAround(_rotationPivot.position, v100, _rotationalSpeed.x * Time.deltaTime);
      transform.RotateAround(_rotationPivot.position, v010, _rotationalSpeed.y * Time.deltaTime);
      transform.RotateAround(_rotationPivot.position, v001, _rotationalSpeed.z * Time.deltaTime);
    }
  }

  private void StopRotating() {
    _enableRotating = false;
  }

  #endregion ===================================================================================================================================


  #region PUBLIC METHODS ===================================================================================================================================

  public void SetPosition(Vector3 pos) { }

  public void SetRotation(Vector3 rot) { }

  public void SetScale(Vector3 scale) { }

  #endregion ===================================================================================================================================
}