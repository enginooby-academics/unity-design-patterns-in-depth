using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEngine.Events;

// ? Make generics for int/float
// ? Stat can be separated from GameManager => Make seperate StatManager

[Serializable, InlineProperty]
public class StatGame {
  [HideInInspector] public string statName;

  public StatGame(string statName, int initialValue = 0, StatEvent triggerGameOverValue = StatEvent.None) {
    this.statName = statName;
    this.InitialValue = initialValue;
    this.triggerGameOverValue = triggerGameOverValue;
    // this.ui.statName = statName;
    this.ui.prefix = statName + ": ";
  }

  private const float LABEL_WIDTH_1 = 80f;

  // [ToggleGroup(nameof(enable), groupTitle: "$statName")]
  [FoldoutGroup("$statName")]
  [HorizontalGroup("$statName/Enable"), LabelWidth(LABEL_WIDTH_1)]
  [LabelText("Enable Stat")]
  public bool enable = true;

  #region VALUES ===================================================================================================================================
  // [ToggleGroup(nameof(enable))]
  [FoldoutGroup("$statName"), EnableIf(nameof(enable))]
  [HorizontalGroup("$statName/Value"), LabelWidth(LABEL_WIDTH_1)]
  [OnValueChanged(nameof(UpdateStatUI))]
  public int InitialValue;
  [HideInInspector] public int CurrentValue;

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
  public int MaxValue;
  private bool EnableStatAndMax() { return enable && enableMax; }
  #endregion ===================================================================================================================================

  // [ToggleGroup(nameof(enable))]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [OnValueChanged(nameof(UpdateStatUI), true)]
  [HideLabel] public StatUI ui = new StatUI();

  private void UpdateStatUI() {
    ui.Update(InitialValue);
  }

  #region EVENTS ===================================================================================================================================
  // TODO: universal FXs on Stat change events (for FXs of specific objects, setup in EventManager)

  public enum StatEvent { Min, Max, Zero, None } // ? Use enum flag

  // [ToggleGroup(nameof(enable))]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [OnValueChanged(nameof(OnTriggerGameOverValueChange))]
  [PropertySpace(SpaceBefore = 10)]
  [EnumToggleButtons, LabelText("Trigger Game Over On")] public StatEvent triggerGameOverValue = StatEvent.None;

  private void OnTriggerGameOverValueChange() {
    switch (triggerGameOverValue) {
      case StatEvent.Min:
        enableMin = true;
        break;
      case StatEvent.Max:
        enableMax = true;
        break;
      default:
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
      default:
        break;
    }
  }

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Manual Events")]
  public UnityEvent OnStatIncrease = new UnityEvent();

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Manual Events")]
  public UnityEvent OnStatDecrease = new UnityEvent();

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Manual Events")]
  public UnityEvent OnStatMin = new UnityEvent();

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Manual Events")]
  public UnityEvent OnStatMax = new UnityEvent();

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Manual Events")]
  public UnityEvent OnStatZero = new UnityEvent();

  public void SetupGameEvents() {
    GameManager.Instance.OnGameLoad.AddListener(OnGameLoad);
    GameManager.Instance.OnGameStart.AddListener(OnGameStart);
    GameManager.Instance.OnGameOver.AddListener(OnGameOver);
  }

  public void OnGameLoad() {
    this.CurrentValue = this.InitialValue;
  }

  public void OnGameStart() {

  }

  public void OnGameOver() {

  }
  #endregion ===================================================================================================================================

  #region PUBLIC METHODS ===================================================================================================================================
  public void Update(int amountToAdd) {
    if (GameManager.Instance.gameOver || !enable) return;

    int valueAfterUpdate = CurrentValue + amountToAdd;
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