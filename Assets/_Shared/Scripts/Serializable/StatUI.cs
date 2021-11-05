// * Display game stat UI
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using TMPro;

[Serializable, InlineProperty]
public class StatUI {
  [HideInInspector] public string statName; // REFACTOR: Use StringBuilder
  public StatUI(string statName = "UI", string prefix = null, string suffix = null) {
    this.statName = statName;
    this.prefix ??= this.statName + ": ";
    this.suffix ??= "";
  }

  public enum UIType { Text, Icon, Progress }
  [BoxGroup("$statName")]
  [HideLabel, EnumToggleButtons] public UIType uiType = UIType.Text;

  public const float LABEL_WIDTH = 45f;

  #region TEXT ===================================================================================================================================
  [BoxGroup("$statName")]
  [LabelWidth(LABEL_WIDTH)]
  [ShowIf(nameof(uiType), UIType.Text)]
  public TextMeshProUGUI label;

  // [BoxGroup("$statName")]
  [HorizontalGroup("$statName/Pre&Suffix")]
  [LabelWidth(LABEL_WIDTH)]
  [ShowIf(nameof(uiType), UIType.Text)]
  public string prefix;

  // [BoxGroup("$statName")]
  // [HorizontalGroup("$statName/Prefix & Suffix")]
  [HorizontalGroup("$statName/Pre&Suffix")]
  [LabelWidth(LABEL_WIDTH)]
  [ShowIf(nameof(uiType), UIType.Text)]
  public string suffix;
  #endregion ===================================================================================================================================

  #region ICON ===================================================================================================================================
  // [ShowIf(nameof(statType), StatType.Icon)]
  // public Image?? statIcon;
  #endregion

  public void OnGameStart() {

  }

  public void OnGameOver() {

  }

  public void Update(int amount) {
    switch (uiType) {
      case UIType.Text:
        label.text = prefix + amount + suffix;
        break;
    }
  }
}