// * Alternative: generic SO class as in SO Architecture asset


using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

[Serializable]
[InlineProperty]
/// <summary>
/// Wrapper for SO variable (reference) and normal variable (value).
/// </summary>
public class ReferenceVariable<T> where T : IEquatable<T> {
  [HorizontalGroup("Reference", MaxWidth = 100)] [ValueDropdown(nameof(valueList))] [SerializeField] [HideLabel]
  private bool _useConstant = true;

  [ShowIf(nameof(_useConstant), Animate = false)] [HorizontalGroup("Reference")] [SerializeField] [HideLabel]
  private T _constantValue;

  [HideIf(nameof(_useConstant), Animate = false)]
  [HorizontalGroup("Reference")]
  [InlineEditor]
  [SerializeField]
  [HideLabel]
  // [Required] // ! Causes editor error
  // ! Drawback with generics: editor doesn't display specific type and cannot search for asset
  // Solution: create ReferenceInt class w/ ReferenceIntSO field.
  private ReferenceVariableSO<T> _variable;


#if ODIN_INSPECTOR
  private ValueDropdownList<bool> valueList = new ValueDropdownList<bool> {
    {"Value", true},
    {"Reference", false}
  };
#else
  private int valueList;
#endif

  public T Value {
    get => _useConstant ? _constantValue : _variable.Value;
    set {
      if (_useConstant)
        _constantValue = value;
      else
        _variable.Value = value;
    }
  }
}