#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using System;
using UnityEngine;

namespace Enginooby.Core {
  // REFACTOR: Create generics class & RandomFloat
  [Serializable]
  [InlineProperty]
  public class RandomInt {
    // TODO: Excluding & including values for non-continuous range 
    // [MinMaxSlider(nameof(_minValue), nameof(_maxValue))] [SerializeField] [HideLabel]
    // private Vector2 range;

    [SerializeField] private int min;
    [SerializeField] private int max;
    private int? _value = null;

    public RandomInt(int min, int max) {
      // range.x = this.min = min;
      // range.y = this.max = max;
      this.min = min;
      this.max = max;
    }

    // ? Use bool instead to check for performance
    public int Value => _value ?? Randomize(); // lazy init

    /// <summary>
    /// Re-randomize the current value and return it.
    /// </summary>
    public int Random => Randomize();

    private int Randomize() {
      _value = UnityEngine.Random.Range(min, max);
      return _value.Value;
    }

    public static implicit operator int(RandomInt @this) => @this.Value;
  }
}