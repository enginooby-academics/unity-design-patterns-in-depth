using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

[Serializable]
[InlineProperty]
public class Translater : SerializableBase {
  [ToggleGroup(nameof(_enableTranslating), "Translating")] [SerializeField]
  private bool _enableTranslating = true;

  [ToggleGroup(nameof(_enableTranslating))] [OnValueChanged(nameof(Init))] [SerializeField] [LabelText("Speed")]
  private Vector3 _translationalSpeed;

  [ToggleGroup(nameof(_enableTranslating))]
  [ShowIf(nameof(_translatingMode), Mode.Auto)]
  [OnValueChanged(nameof(Init))]
  [SerializeField]
  [LabelText("Min Position")]
  private Vector3 _translationalMinPos;

  [ToggleGroup(nameof(_enableTranslating))]
  [ShowIf(nameof(_translatingMode), Mode.Auto)]
  [OnValueChanged(nameof(Init))]
  [SerializeField]
  [LabelText("Max Position")]
  private Vector3 _translationalMaxPos;

  [ToggleGroup(nameof(_enableTranslating))]
  [ShowIf(nameof(_translatingMode), Mode.Auto)]
  [SerializeField]
  [LabelText("On Reach Boundary")]
  private OnBoundaryAction _translationalOnBoundaryAction = OnBoundaryAction.LoopPingPong;

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

  private Vector3 _originalPos;
  private Transform _transform;
  private bool? IsTranslatingToMaxY;

  public Vector3 TranslationalSpeed {
    get => _translationalSpeed;
    set => _translationalSpeed = value;
  }

  private bool IsTranslatingOnY => _translationalSpeed.y != 0;

  public void Setup(Transform transform) {
    _transform = transform;
  }

  public void Init() {
    _originalPos = _transform.position;
    IsTranslatingToMaxY = _translationalSpeed.y > 0;
  }

  public void InvertTranslationalY() {
    IsTranslatingToMaxY = !IsTranslatingToMaxY;
  }

  public void InvertTranslationalDirection() {
    InvertTranslationalY();
  }

  public void DrawGizmosTranslating() {
    if (!_enableTranslating) return;

    // gameObject.DrawGizmosDirection(_translationalSpeed);
    if (IsTranslatingOnY) {
      Gizmos.DrawSphere(_originalPos.WithY(_translationalMinPos.y), .2f);
      Gizmos.DrawSphere(_originalPos.WithY(_translationalMaxPos.y), .2f);
    }
  }

  public void ProcessTranslating() {
    if (!_enableTranslating) return;

    if (_translatingMode == Mode.Auto) {
      ProcessAutoTranslating();
    }
    else {
      _transform.MoveXWorld(_xTranslateKey.InputValue * _translationalSpeed.x);
      _transform.MoveYWorld(_yTranslateKey.InputValue * _translationalSpeed.y);
      _transform.MoveZWorld(_zTranslateKey.InputValue * _translationalSpeed.z);
    }
  }

  private void ProcessAutoTranslating() {
    if (IsTranslatingOnY) ProcessTranslationalBoundaryY();
    _transform.MoveWorld(_translationalSpeed);
  }

  private void ProcessTranslationalBoundaryY() {
    IsTranslatingToMaxY = _transform.ReachingYMinOrMax(_translationalMinPos.y, _translationalMaxPos.y) ??
                          IsTranslatingToMaxY;
    switch (_translationalOnBoundaryAction) {
      case OnBoundaryAction.LoopPingPong:
        _translationalSpeed = IsTranslatingToMaxY != null && IsTranslatingToMaxY.Value
          ? _translationalSpeed.WithPositiveY()
          : _translationalSpeed.WithNegativeY();
        break;
    }
  }

  public void StopTranslating() {
    _enableTranslating = false;
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
}