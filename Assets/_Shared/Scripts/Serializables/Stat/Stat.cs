// * Alternative: Var Reference SO

// ? Make generic for int/float...
// ? Create subclasses (or Bridge pattern) for: StatWithEvent, StatWithUI, StatWithEventAndUI, Stat (w/o event, UI)
// ? Partial classes

using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

[Serializable, InlineProperty]
public class Stat {
  [HideInInspector] public string statName;

  public Stat(string statName, int initialValue = 0) {
    this.statName = statName;
    this.InitialValue = initialValue;
    this.CurrentValue = initialValue;
    // this.ui.statName = statName;
    this.uis.ForEach(ui => ui.prefix = this.statName + ": ");
  }

  // REFACTOR
  public Stat(StatName statName, int initialValue = 0) {
    this.statName = statName.ToString();
    this.InitialValue = initialValue;
    this.CurrentValue = initialValue;
    // this.ui.statName = statName;
    this.uis.ForEach(ui => ui.prefix = this.statName + ": ");
  }

  private const float LABEL_WIDTH_1 = 80f;

  [FoldoutGroup("$statName"), EnableIf(nameof(enable))]
  [HorizontalGroup("$statName/Debug"), LabelWidth(LABEL_WIDTH_1)]
  // [ProgressBar(nameof(MinValue), nameof(MaxValue), r: 1, g: 1, b: 1, Height = 30)]
  [DisplayAsString]
  public int CurrentValue;

  [FoldoutGroup("$statName")]
  // [EnableIf(nameof(EnableStatAndMax))]
  // [HorizontalGroup("$statName/Debug"), LabelWidth(LABEL_WIDTH_1)]
  // [Button]
  public float CurrentPercentage => CurrentValue * 100 / MaxValue;


  // [ToggleGroup(nameof(enable), groupTitle: "$statName")]
  [FoldoutGroup("$statName")]
  [HorizontalGroup("$statName/Enable"), LabelWidth(LABEL_WIDTH_1)]
  [LabelText("Enable Stat")]
  public bool enable = true;

  #region VALUES ===================================================================================================================================
  // [ToggleGroup(nameof(enable))]
  [FoldoutGroup("$statName"), EnableIf(nameof(enable))]
  [HorizontalGroup("$statName/Value"), LabelWidth(LABEL_WIDTH_1)]
  [OnValueChanged(nameof(OnInitialValueChanged))]
  public int InitialValue;

  private void OnInitialValueChanged() {
    CurrentValue = InitialValue;
    InitStatUIs();
  }

  // [ToggleGroup(nameof(enable))]
  [FoldoutGroup("$statName"), EnableIf(nameof(enable))]
  [HorizontalGroup("$statName/Enable"), LabelWidth(LABEL_WIDTH_1)]
  public bool enableMin = true;

  [FoldoutGroup("$statName"), EnableIf(nameof(EnableStatAndMin))]
  [HorizontalGroup("$statName/Value"), LabelWidth(LABEL_WIDTH_1)]
  public int MinValue;
  private bool EnableStatAndMin() { return enable && enableMin; }

  [FoldoutGroup("$statName"), EnableIf(nameof(enable))]
  [HorizontalGroup("$statName/Enable"), LabelWidth(LABEL_WIDTH_1)]
  public bool enableMax;

  [FoldoutGroup("$statName"), EnableIf(nameof(EnableStatAndMax))]
  [HorizontalGroup("$statName/Value"), LabelWidth(LABEL_WIDTH_1)]
  [PropertySpace(SpaceAfter = 10, SpaceBefore = 0)]
  public int MaxValue = 100;
  private bool EnableStatAndMax() => enable && enableMax;
  private bool EnableStatAndDisableMax() => enable && !enableMax;
  #endregion ===================================================================================================================================

  // TODO: Implement multiple UIs (? scriptable objects)
  // [ToggleGroup(nameof(enable))]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [OnValueChanged(nameof(InitStatUIs), true)]
  [LabelText("UIs")]
  public List<StatUI> uis = new List<StatUI>() { new StatUI() };

  private void InitStatUIs() {
    UpdateStatUIs(InitialValue);
    // this.uis.ForEach(ui => ui.prefix = statName + ": ");
  }

  private void UpdateStatUIs(int currentValue) {
    Nullable<int> maxValue = (enableMax) ? (int?)MaxValue : null;
    Nullable<int> minValue = (enableMin) ? (int?)MinValue : null;
    uis.ForEach(ui => ui.Update(currentValue, maxValue, minValue));
  }

  #region EVENTS ===================================================================================================================================
  // TIP: Declare both public C# event and private serialized UnityEvent 
  // -> Use C# event (bind in script) instead of UnityEvent (bind in Inspector) for better performance.

  // ? Rename events: changed, increased, decreased 

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Events")]
  public UnityEvent OnStatChange = new UnityEvent();
  public event Action OnStatChangeEvent;

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Events")]
  public UnityEvent OnStatIncrease = new UnityEvent();
  public event Action OnStatIncreaseEvent;

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Events")]
  public UnityEvent OnStatDecrease = new UnityEvent();
  public event Action OnStatDecreaseEvent;

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Events")]
  public UnityEvent OnStatMin = new UnityEvent();
  public event Action OnStatMinEvent;

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Events")]
  public UnityEvent OnStatMax = new UnityEvent();
  public event Action OnStatMaxEvent;

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Events")]
  public UnityEvent OnStatZero = new UnityEvent();
  public event Action OnStatZeroEvent;
  #endregion ===================================================================================================================================

  #region PUBLIC METHODS ===================================================================================================================================
  // ? Rename to Add()
  [ObsoleteAttribute("This is obsolete. Use Add instead.", false)]
  public void Update(int amountToAdd) {
    Set(CurrentValue + amountToAdd);
  }

  public void Add(int amount) => Set(CurrentValue + amount);

  // ? Use a singleton MonoBehaviour instead, w/ extension method: Coroutine.Start()
  /// <summary>
  /// Add temporarily for a period of time, then return to the previous value. 
  /// Provide MonoBehaviour to start coroutine.
  /// </summary>
  public void Add(int amount, float duration, MonoBehaviour monoBehaviour) {
    monoBehaviour.StartCoroutine(AddCoroutine(amount, duration));
  }

  private IEnumerator AddCoroutine(int amount, float duration) {
    Add(amount);
    yield return new WaitForSeconds(duration);
    Add(-amount);
  }

  /// <summary>
  /// Increase current stat value by 1.
  /// </summary>
  public void Increase() => Add(1);

  /// <summary>
  /// Descrease current stat value by 1.
  /// </summary>
  public void Descrease() => Add(-1);

  public void SetZero() => Set(0);
  public void SetMin() => Set(MinValue);
  public void SetMax() => Set(MaxValue);

  /// <summary>
  /// Constraint the given amount in range of min-max (if enable) before setting value.
  /// </summary>
  // REFACTOR
  public void Set(int value) {
    if (enableMin && value < MinValue) {
      value = MinValue;
    }
    if (enableMax && value > MaxValue) {
      value = MaxValue;
    }

    if (value == CurrentValue) return;
    OnStatChange.Invoke();
    OnStatChangeEvent?.Invoke();

    int oldValue = CurrentValue;
    CurrentValue = value;
    UpdateStatUIs(CurrentValue);

    if (CurrentValue > oldValue) {
      OnStatIncrease.Invoke();
      OnStatIncreaseEvent?.Invoke();
    }
    if (CurrentValue < oldValue) {
      OnStatDecrease.Invoke();
      OnStatDecreaseEvent?.Invoke();
    }
    if (CurrentValue == 0) {
      OnStatZero.Invoke();
      OnStatZeroEvent?.Invoke();
    }
    if (enableMin && CurrentValue <= MinValue) {
      OnStatMin.Invoke();
      OnStatMinEvent?.Invoke();
    }
    if (enableMax && CurrentValue >= MaxValue) {
      OnStatMax.Invoke();
      OnStatMaxEvent?.Invoke();
    }
  }
  #endregion ===================================================================================================================================
}
