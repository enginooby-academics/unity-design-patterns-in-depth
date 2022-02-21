using System;
using UnityEngine;
using UnityEngine.Events;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

// TODO: Extend from Stat, couple w/ GameManager for quick prototyping
[Serializable]
[InlineProperty]
public class StatGame {
  private const float LABEL_WIDTH_1 = 80f;
  [HideInInspector] public string statName;

  // [ToggleGroup(nameof(enable), groupTitle: "$statName")]
  [FoldoutGroup("$statName")]
  [HorizontalGroup("$statName/Enable")]
  [LabelWidth(LABEL_WIDTH_1)]
  [LabelText("Enable Stat")]
  public bool enable = true;

  // [ToggleGroup(nameof(enable))]
  [FoldoutGroup("$statName")] [ShowIf(nameof(enable))] [OnValueChanged(nameof(UpdateStatUI), true)] [HideLabel]
  public StatUI ui = new();

  public StatGame(string statName, int initialValue = 0, StatEvent triggerGameOverValue = StatEvent.None) {
    this.statName = statName;
    InitialValue = initialValue;
    this.triggerGameOverValue = triggerGameOverValue;
    // this.ui.statName = statName;
    ui.prefix = statName + ": ";
  }

  private void UpdateStatUI() {
    ui.Update(InitialValue);
  }

  #region VALUES ===================================================================================================================================

  // [ToggleGroup(nameof(enable))]
  [FoldoutGroup("$statName")]
  [EnableIf(nameof(enable))]
  [HorizontalGroup("$statName/Value")]
  [LabelWidth(LABEL_WIDTH_1)]
  [OnValueChanged(nameof(UpdateStatUI))]
  public int InitialValue;

  [HideInInspector] public int CurrentValue;

  // [ToggleGroup(nameof(enable))]
  [FoldoutGroup("$statName")]
  [EnableIf(nameof(enable))]
  [HorizontalGroup("$statName/Enable")]
  [LabelWidth(LABEL_WIDTH_1)]
  public bool enableMin = true;

  [FoldoutGroup("$statName")]
  [EnableIf(nameof(EnableStatAndMin))]
  [HorizontalGroup("$statName/Value")]
  [LabelWidth(LABEL_WIDTH_1)]
  public int MinValue;

  private bool EnableStatAndMin() => enable && enableMin;

  [FoldoutGroup("$statName")]
  [EnableIf(nameof(enable))]
  [HorizontalGroup("$statName/Enable")]
  [LabelWidth(LABEL_WIDTH_1)]
  public bool enableMax;

  [FoldoutGroup("$statName")]
  [EnableIf(nameof(EnableStatAndMax))]
  [HorizontalGroup("$statName/Value")]
  [LabelWidth(LABEL_WIDTH_1)]
  [PropertySpace(SpaceAfter = 10, SpaceBefore = 0)]
  public int MaxValue;

  private bool EnableStatAndMax() => enable && enableMax;

  #endregion ===================================================================================================================================

  #region EVENTS ===================================================================================================================================

  // TODO: universal FXs on Stat change events (for FXs of specific objects, setup in EventManager)

  public enum StatEvent {
    Min,
    Max,
    Zero,
    None,
  } // ? Use enum flag

  // [ToggleGroup(nameof(enable))]
  [FoldoutGroup("$statName")]
  [ShowIf(nameof(enable))]
  [OnValueChanged(nameof(OnTriggerGameOverValueChange))]
  [PropertySpace(SpaceBefore = 10)]
  [EnumToggleButtons]
  [LabelText("Trigger Game Over On")]
  public StatEvent triggerGameOverValue = StatEvent.None;

  private void OnTriggerGameOverValueChange() {
    switch (triggerGameOverValue) {
      case StatEvent.Min:
        enableMin = true;
        break;
      case StatEvent.Max:
        enableMax = true;
        break;
    }
  }

  private void ProcessTriggerGameOverValue() {
    switch (triggerGameOverValue) {
      case StatEvent.Min:
        if (enableMin && CurrentValue == MinValue) GameManager.Instance.SetGameOver();
        break;
      case StatEvent.Max:
        if (enableMax && CurrentValue == MaxValue) GameManager.Instance.SetGameOver();
        break;
      case StatEvent.Zero:
        if (CurrentValue == 0) GameManager.Instance.SetGameOver();
        break;
    }
  }

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName")] [ShowIf(nameof(enable))] [FoldoutGroup("$statName/Manual Events")]
  public UnityEvent OnStatIncrease = new();

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName")] [ShowIf(nameof(enable))] [FoldoutGroup("$statName/Manual Events")]
  public UnityEvent OnStatDecrease = new();

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName")] [ShowIf(nameof(enable))] [FoldoutGroup("$statName/Manual Events")]
  public UnityEvent OnStatMin = new();

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName")] [ShowIf(nameof(enable))] [FoldoutGroup("$statName/Manual Events")]
  public UnityEvent OnStatMax = new();

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName")] [ShowIf(nameof(enable))] [FoldoutGroup("$statName/Manual Events")]
  public UnityEvent OnStatZero = new();

  public void SetupGameEvents() {
    GameManager.Instance.OnGameLoad.AddListener(OnGameLoad);
    GameManager.Instance.OnGameStart.AddListener(OnGameStart);
    GameManager.Instance.OnGameOver.AddListener(OnGameOver);
  }

  public void OnGameLoad() {
    CurrentValue = InitialValue;
  }

  public void OnGameStart() { }

  public void OnGameOver() { }

  #endregion ===================================================================================================================================

  #region PUBLIC METHODS ===================================================================================================================================

  public void Update(int amountToAdd) {
    if (GameManager.Instance.gameOver || !enable) return;

    var valueAfterUpdate = CurrentValue + amountToAdd;
    if (enableMin && valueAfterUpdate < MinValue || enableMax && valueAfterUpdate > MaxValue) return;

    Set(valueAfterUpdate);
    if (amountToAdd > 0) OnStatIncrease.Invoke();
    if (amountToAdd < 0) OnStatDecrease.Invoke();
  }

  public void Increase() {
    Update(1);
  }

  public void Descrease() {
    Update(-1);
  }

  public void Set(int value) {
    CurrentValue = value;
    ui.Update(CurrentValue);
    ProcessTriggerGameOverValue();
  }

  public void SetZero() {
    Set(0);
    OnStatZero.Invoke();
  }

  public void SetMin() {
    Set(MinValue);
    OnStatMin.Invoke();
  }

  public void SetMax() {
    Set(MaxValue);
    OnStatMax.Invoke();
  }

  #endregion ===================================================================================================================================
}