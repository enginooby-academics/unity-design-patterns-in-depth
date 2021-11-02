// * Use cases: for simple transformations (w/ its loop & constrain). E.g: bullet, non-physical controller
// * For complex movements, use path tools such as Simple Waypoint System
using System.Collections;
using System.Collections.Generic;
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
    if (enableLifetime) Destroy(gameObject, lifetime);
  }

  void LateUpdate() {
    // ProcessLifetime();
    ProcessTranslating();
    ProcessRotating();
  }

  public void Stop() {
    StopTranslating();
    StopRotating();
  }

  void OnDrawGizmosSelected() {
    DrawGizmosTranslating();
  }

  // ? make Lifetime Serializable class or move to MonoBehaviourBase
  [SerializeField] bool enableLifetime;
  [SerializeField, Min(0f)] float lifetime;
  void ProcessLifetime() { // Alternative: Destroy w/ delay
    if (!enableLifetime) return;

    lifetime -= Time.deltaTime;
    if (lifetime <= 0f) Destroy(gameObject);
  }

  #region TRANSLATING ===================================================================================================================================
  [ToggleGroup(nameof(enableTranslating), groupTitle: "Translating")]
  [SerializeField] bool enableTranslating = true;
  [ToggleGroup(nameof(enableTranslating))]
  [SerializeField, LabelText("Speed")] Vector3 translationalSpeed;
  [ToggleGroup(nameof(enableTranslating))]
  [SerializeField, LabelText("Acceleration")] Vector3 translationalAcceleration;
  [ToggleGroup(nameof(enableTranslating))]
  [SerializeField, LabelText("Offset Bound")] Vector3 translationalOffsetBound;

  [ToggleGroup(nameof(enableTranslating))]
  [SerializeField, EnumToggleButtons] Mode translatingMode = Mode.Auto;

  [ToggleGroup(nameof(enableTranslating))]
  [ShowIf(nameof(translatingMode), Mode.Control)]
  [SerializeField] InputModifier xTranslateKey = new InputModifier(inputType: InputModifier.InputType.Axis);
  [ToggleGroup(nameof(enableTranslating))]
  [ShowIf(nameof(translatingMode), Mode.Control)]
  [SerializeField] InputModifier yTranslateKey = new InputModifier(inputType: InputModifier.InputType.Axis);
  [ToggleGroup(nameof(enableTranslating))]
  [ShowIf(nameof(translatingMode), Mode.Control)]
  [SerializeField] InputModifier zTranslateKey = new InputModifier(inputType: InputModifier.InputType.Axis, inputAxis: InputAxis.Horizontal);

  void DrawGizmosTranslating() {
    if (!enableTranslating) return;

    gameObject.DrawGizmosDirection(translationalSpeed);
    Gizmos.DrawSphere(gameObject.transform.position + translationalOffsetBound, .2f);
  }

  void ProcessTranslating() {
    if (!enableTranslating) return;

    if (translatingMode == Mode.Auto) {
      this.MoveWorld(distances: translationalSpeed);
    } else {
      this.MoveXWorld(distance: xTranslateKey.InputValue * translationalSpeed.x);
      this.MoveYWorld(distance: yTranslateKey.InputValue * translationalSpeed.y);
      this.MoveZWorld(distance: zTranslateKey.InputValue * translationalSpeed.z);
    }
  }

  void StopTranslating() {
    enableTranslating = false;
  }
  #endregion ===================================================================================================================================

  #region ROTATING ===================================================================================================================================
  [ToggleGroup(nameof(enableRotating), groupTitle: "Rotating")]
  [SerializeField] bool enableRotating;

  [ToggleGroup(nameof(enableRotating))]
  [SerializeField, LabelText("Speed")] Vector3 rotationalSpeed;
  [ToggleGroup(nameof(enableRotating))]
  [SerializeField, LabelText("Acceleration")] Vector3 rotationalAcceleration;

  [ToggleGroup(nameof(enableRotating))]
  [InlineButton(nameof(SetSelfAsRotationPivot), "Self")]
  [SerializeField, LabelText("Pivot")] Transform rotationPivot;
  private void SetSelfAsRotationPivot() {
    rotationPivot = transform;
  }

  [ToggleGroup(nameof(enableRotating))]
  [SerializeField, EnumToggleButtons] Mode rotationMode = Mode.Auto;

  [ToggleGroup(nameof(enableRotating))]
  [ShowIf(nameof(rotationMode), Mode.Control)]
  [SerializeField] InputModifier xRotateKey = new InputModifier();
  [ToggleGroup(nameof(enableRotating))]
  [ShowIf(nameof(rotationMode), Mode.Control)]
  [SerializeField] InputModifier yRotateKey = new InputModifier();
  [ToggleGroup(nameof(enableRotating))]
  [ShowIf(nameof(rotationMode), Mode.Control)]
  [SerializeField] InputModifier zRotateKey = new InputModifier();

  void ProcessRotating() {
    if (!enableRotating) return;

    if (rotationPivot) ProcessRotatingWithPivot();
    else ProcessRotatingWithoutPivot();
  }

  void ProcessRotatingWithoutPivot() {
    if (rotationMode == Mode.Control) {
      this.RotateX(rotationalSpeed.x * xRotateKey.InputValue);
      this.RotateY(rotationalSpeed.y * yRotateKey.InputValue);
      this.RotateZ(rotationalSpeed.z * zRotateKey.InputValue);
    } else {
      this.Rotate(angles: rotationalSpeed);
    }
  }

  void ProcessRotatingWithPivot() {
    if (rotationMode == Mode.Control) {
      transform.RotateAround(rotationPivot.position, v100, rotationalSpeed.x * Time.deltaTime * xRotateKey.InputValue);
      transform.RotateAround(rotationPivot.position, v010, rotationalSpeed.y * Time.deltaTime * yRotateKey.InputValue);
      transform.RotateAround(rotationPivot.position, v001, rotationalSpeed.z * Time.deltaTime * zRotateKey.InputValue);
    } else {
      transform.RotateAround(rotationPivot.position, v100, rotationalSpeed.x * Time.deltaTime);
      transform.RotateAround(rotationPivot.position, v010, rotationalSpeed.y * Time.deltaTime);
      transform.RotateAround(rotationPivot.position, v001, rotationalSpeed.z * Time.deltaTime);
    }
  }

  void StopRotating() {
    enableRotating = false;
  }
  #endregion ===================================================================================================================================

  #region SCALING ===================================================================================================================================
  [ToggleGroup(nameof(enableScaling), groupTitle: "Scaling")]
  [SerializeField] bool enableScaling;

  #endregion ===================================================================================================================================


  #region PUBLIC METHODS ===================================================================================================================================
  public void SetPosition(Vector3 pos) {

  }

  public void SetRotation(Vector3 rot) {

  }

  public void SetScale(Vector3 scale) {

  }

  public void CopyTransform(Transform transformToCopy) {
    transform.position = transformToCopy.position;
    transform.rotation = transformToCopy.rotation;
    transform.localScale = transformToCopy.localScale;
  }
  #endregion ===================================================================================================================================

}
