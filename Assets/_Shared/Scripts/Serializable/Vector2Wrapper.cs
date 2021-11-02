// * Wrapper for UnityEngine.Vector2 to provide extra functions/properties, helper inline buttons in Inspector
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEngine.Events;

[Serializable, InlineProperty]
public class Vector2Wrapper {
  [HideInInspector] public UnityEvent OnValueChange = new UnityEvent();

  [MinMaxSlider(nameof(min), nameof(max), true)]
  [InlineButton(nameof(ToggleShowMore), "...")]
  [InlineButton(nameof(ToMax), ">")]
  [InlineButton(nameof(ToMin), "<")]
  [InlineButton(nameof(IncreaseRange), "-+")]
  [InlineButton(nameof(DecreaseRange), "+-")]
  [InlineButton(nameof(ToZero), "0")]
  [HideLabel]
  public Vector2 Value;

  [HideInInspector] bool showMore;

  [HorizontalGroup("Dynamic Min Max")]
  [ShowIf(nameof(showMore))]
  public float min = -10f;

  [HorizontalGroup("Dynamic Min Max")]
  [ShowIf(nameof(showMore))]
  public float max = 10f;

  public Vector2Wrapper(Vector2? value = null, float min = -10f, float max = 10f) {
    this.Value = value ?? Vector2.zero;
    this.min = min;
    this.max = max;
    OnValueChange.AddListener(ToZero);
  }

  #region INLINE BUTTONS ===================================================================================================================================
  public void ToZero() { Value = Vector2.zero; }
  // TODO: constraint Increase() & Decrease()
  public void IncreaseRange() { Value = new Vector2(Value.x - 1, Value.y + 1); }
  public void DecreaseRange() { Value = new Vector2(Value.x + 1, Value.y - 1); }
  public void ToMax() { Value = new Vector2(Value.y, Value.y); }
  public void ToMin() { Value = new Vector2(Value.x, Value.x); }
  public void ToggleShowMore() { showMore = !showMore; }
  #endregion ===================================================================================================================================

  #region PROPERTIES/FUNCTIONS ===================================================================================================================================
  public float Random { get => Value.Random(); }
  public int RandomInt { get => (int)Value.Random(); }
  public float Clamp(float num) { return Value.Clamp(num); }

  /// <summary>
  /// Return true if Vector2.zero, used to treat unset Vector as infinite Vector (e.g. in Boundary)
  /// </summary>
  public bool ContainsIgnoreZero(float value) { return Value.ContainsIgnoreZero(value); }
  public bool IsZero { get => Value == Vector2.zero; }
  public float Average { get => Value.Average(); }
  public float Length { get => Value.Length(); }
  #endregion ===================================================================================================================================
}