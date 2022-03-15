//* Help to get single reference by different methods: tag, name, type, ref (direct assign in Inspector)... in Scene or Asset
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginooby.Attribute;
#endif

using UnityEngine;
using System;

// ? Derived classes: ReferenceVector3/ReferenceTag has additional Vector3 targetVect/string tag attribute... 
// ? Filter
// ? Rename to ReferenceGameObject
[Serializable, InlineProperty]
public class Reference : SerializableBase {
  public enum FindMethod { Ref, Tag, Name, Type }
  // [Title("Target", bold: false, horizontalLine: false), PropertySpace(spaceBefore: -10)]

  protected bool _enableGameObjectReference = true;

  [EnableIf(nameof(_enableGameObjectReference))]
  [ShowIf(nameof(findMethod), FindMethod.Ref)]
  [InlineButton(nameof(SetPlayerAsTarget), "Player")]
  [InlineButton(nameof(SetSelfAsTarget), "Self")]
  [SerializeField, HideLabel]
  protected GameObject _gameObjectRef;

  private void SetPlayerAsTarget() {
    _gameObjectRef = GameObject.FindGameObjectWithTag("Player");
    if (!_gameObjectRef) _gameObjectRef = GameObject.Find("Player");
    if (!_gameObjectRef) Debug.LogWarning("GameObject with Player tag/name does not exist.");
  }

  private void SetSelfAsTarget() {
    if (!base.GameObject) return;
    _gameObjectRef = base.GameObject;
  }

  [EnableIf(nameof(_enableGameObjectReference))]
  [ShowIf(nameof(findMethod), FindMethod.Tag)]
  // [Enginooby.Attribute.Tag]
  [HideLabel, SerializeField]
  private string _tag = "Player";
  [EnableIf(nameof(_enableGameObjectReference))]
  [ShowIf(nameof(findMethod), FindMethod.Name)]
  [HideLabel, SerializeField]
  private string _name = "Player";
  [EnableIf(nameof(_enableGameObjectReference))]
  [ShowIf(nameof(findMethod), FindMethod.Type)]
  [HideLabel, SerializeField]
  private string _type;

  public string Tag => _tag;
  public string Name => _name;
  public string Type => _type;

  [EnableIf(nameof(_enableGameObjectReference))]
  [EnumToggleButtons, HideLabel]
  public FindMethod findMethod;

  public new GameObject GameObject {
    get {
      if (!_gameObjectRef) FindTarget();
      return _gameObjectRef;
    }
    set {
      if (value) _gameObjectRef = value;
    }
  }

  // UTIL
  private void FindTarget() {
    if (findMethod == FindMethod.Tag) _gameObjectRef = GameObject.FindGameObjectWithTag(_tag);
    if (findMethod == FindMethod.Name) _gameObjectRef = GameObject.Find(_name);
    // if (targetFindMethod == FindMethod.Name) target = GameObject.FindObjectOfType<GetType // IMPL

    if (_gameObjectRef) {
      _name = _gameObjectRef.name;
      _tag = _gameObjectRef.tag;
    }
  }
}