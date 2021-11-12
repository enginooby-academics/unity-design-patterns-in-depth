//* Help to get single reference by different methods: tag, name, type, ref (direct assign in Inspector)... in Scene or Asset

using UnityEngine;
using Sirenix.OdinInspector;
using System;


// ? Derived classes: ReferenceVector3/ReferenceTag has additional Vector3 targetVect/string tag attribute... 
// ? Filter

[Serializable, InlineProperty]
public class Reference : SerializableBase {
  public enum FindMethod { Ref, Tag, Name, Type }
  // [Title("Target", bold: false, horizontalLine: false), PropertySpace(spaceBefore: -10)]

  [ShowIf(nameof(findMethod), FindMethod.Ref)]
  [InlineButton(nameof(SetPlayerAsTarget), "Player")]
  [InlineButton(nameof(SetSelfAsTarget), "Self")]
  [SerializeField, HideLabel] GameObject gameObjectRef;
  private void SetPlayerAsTarget() {
    gameObjectRef = GameObject.FindGameObjectWithTag("Player");
    if (!gameObjectRef) gameObjectRef = GameObject.Find("Player");
    if (!gameObjectRef) Debug.LogWarning("GameObject with Player tag/name does not exist.");
  }

  private void SetSelfAsTarget() {
    if (!componentOwner) return;
    gameObjectRef = componentOwner;
  }

  [ShowIf(nameof(findMethod), FindMethod.Tag)]
  [NaughtyAttributes.Tag]
  [HideLabel] public string tag = "Player";

  [ShowIf(nameof(findMethod), FindMethod.Name)]
  [HideLabel] public string name = "Player";

  [ShowIf(nameof(findMethod), FindMethod.Type)]
  [HideLabel] public string type;

  [EnumToggleButtons, HideLabel] public FindMethod findMethod;

  public GameObject GameObject {
    get {
      if (!gameObjectRef) FindTarget();
      return gameObjectRef;
    }
    set {
      if (value) gameObjectRef = value;
    }
  }

  // UTIL
  private void FindTarget() {
    if (findMethod == FindMethod.Tag) gameObjectRef = GameObject.FindGameObjectWithTag(tag);
    if (findMethod == FindMethod.Name) gameObjectRef = GameObject.Find(name);
    // if (targetFindMethod == FindMethod.Name) target = GameObject.FindObjectOfType<GetType // IMPL
  }
}