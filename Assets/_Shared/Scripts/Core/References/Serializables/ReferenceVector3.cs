using UnityEngine;
using Sirenix.OdinInspector;
using System;

// ? Rename to ReferencePosition - wrapper for: gameObject.position, Vector3, Vector3 SO

[Serializable, InlineProperty]
public class ReferenceVector3 : Reference {
  [HorizontalGroup("Vector 3")]
  [OnValueChanged(nameof(OnUseStaticVector3Changed))]
  [SerializeField, ToggleLeft, LabelText("Static Vector3")]
  private bool _useStaticVector3;

  [HorizontalGroup("Vector 3")]
  [SerializeField, HideLabel]
  [EnableIf(nameof(_useStaticVector3))]
  private Vector3 _value;

  public ReferenceVector3() {
  }

  public ReferenceVector3(Vector3 value) {
    SetStaticValue(value);
  }

  public ReferenceVector3(GameObject gameObject) {
    SetOrigin(gameObject);
  }

  public void SetStaticValue(Vector3 value) {
    _value = value;
    _useStaticVector3 = true;
    OnUseStaticVector3Changed();
  }

  public void SetOrigin(GameObject gameObject) {
    _gameObjectRef = gameObject;
    _useStaticVector3 = false;
    OnUseStaticVector3Changed();
  }

  public Vector3 Value {
    get {
      if (_useStaticVector3) return _value;
      else return GameObject.transform.position;
    }
  }

  public bool HasValue {
    get {
      if (_useStaticVector3 || GameObject) return true;
      return false;
    }
  }

  private void OnUseStaticVector3Changed() {
    _enableGameObjectReference = !_useStaticVector3;
  }
}