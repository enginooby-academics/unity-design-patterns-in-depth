// * Wrapper for UnityEngine.Vector2 to provide extra functions/properties, helper inline buttons in Inspector


using System;
using UnityEngine;
using UnityEngine.Events;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

[Serializable]
[InlineProperty]
public class Vector2Wrapper {
  [HideInInspector] public UnityEvent OnValueChange = new();

  [MinMaxSlider(nameof(min), nameof(max), true)]
  [InlineButton(nameof(ToggleShowMore), "...")]
  [InlineButton(nameof(ToMax), ">")]
  [InlineButton(nameof(ToMin), "<")]
  [InlineButton(nameof(IncreaseRange), "-+")]
  [InlineButton(nameof(DecreaseRange), "+-")]
  [InlineButton(nameof(ToZero), "0")]
  [HideLabel]
  public Vector2 Value;

  [HorizontalGroup("Dynamic Min Max")] [ShowIf(nameof(showMore))]
  public float min = -10f;

  [HorizontalGroup("Dynamic Min Max")] [ShowIf(nameof(showMore))]
  public float max = 10f;

  [HideInInspector] private bool showMore;

  public Vector2Wrapper(Vector2? value = null, float min = -10f, float max = 10f) {
    Value = value ?? Vector2.zero;
    this.min = min;
    this.max = max;
    OnValueChange.AddListener(ToZero);
  }

  #region INLINE BUTTONS ===================================================================================================================================

  public void ToZero() {
    Value = Vector2.zero;
  }

  // TODO: constraint Increase() & Decrease()
  public void IncreaseRange() {
    Value = new Vector2(Value.x - 1, Value.y + 1);
  }

  public void DecreaseRange() {
    Value = new Vector2(Value.x + 1, Value.y - 1);
  }

  public void ToMax() {
    Value = new Vector2(Value.y, Value.y);
  }

  public void ToMin() {
    Value = new Vector2(Value.x, Value.x);
  }

  public void ToggleShowMore() {
    showMore = !showMore;
  }

  #endregion ===================================================================================================================================

  #region PROPERTIES/FUNCTIONS ===================================================================================================================================

  public float Random => Value.Random();
  public int RandomInt => (int) Value.Random();

  public float Clamp(float num) => Value.Clamp(num);

  /// <summary>
  ///   Return true if Vector2.zero, used to treat unset Vector as infinite Vector (e.g. in Boundary)
  /// </summary>
  public bool ContainsIgnoreZero(float value) => Value.ContainsIgnoreZero(value);

  public bool IsZero => Value == Vector2.zero;
  public float Average => Value.Average();
  public float Length => Value.Length();

  #endregion ===================================================================================================================================
}