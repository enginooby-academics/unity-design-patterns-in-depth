/// Use cases: random bullets/vfx/sfx, postfx profile/character switcher in Play/Edit Mode

using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using System.Linq;

// ? Convert to SO
// TODO:
// + AssetType: Prefab, Scene, Both
// + Create CollectionItem class w/ inline buttons, probability for random mode, isFavorite, customize
// + FavoriteItems as sublist of items
// + Project assets by provide folder path
// + Tags, filter...for each item

[Serializable, InlineProperty]
public class AssetCollection<T> where T : UnityEngine.Object {
  #region INSPECTOR FORMATING
  const float LINE_SPACE = 5;
  const float LABEL_WIDTH_KEY = 100;
  #endregion

  [OnValueChanged(nameof(OnItemsChanged))]
  public List<T> items = new List<T>();
  [OnValueChanged(nameof(OnCurrentItemChanged))]
  [InlineButton(nameof(GetAndSetToRandomItem), "?")]
  [InlineButton(nameof(GetAndSetToNextItem), ">")]
  [InlineButton(nameof(GetAndSetToPreviousItem), "<")]
  [SerializeField, ValueDropdown(nameof(items)), Space(LINE_SPACE)]
  T currentItem;

  public enum CollectionRetrieveMode { Current, Random, Iterate }

  [SerializeField, EnumToggleButtons, Space(LINE_SPACE)]
  public CollectionRetrieveMode retrieveMode = CollectionRetrieveMode.Random;

  [PropertySpace(SpaceAfter = 10, SpaceBefore = 0)]
  [OnValueChanged(nameof(UpdateDeactivateExceptCurrent))]
  [LabelText("Deactivate In-Scene Items (Excluding Current)")]
  [SerializeField, ToggleLeft, Space(LINE_SPACE)]
  public bool deactivateExceptCurrent = false;

  #region EVENT
  [ToggleGroup(nameof(enableEvent), groupTitle: "EVENT")]
  public bool enableEvent = true;
  [ToggleGroup(nameof(enableEvent))]
  [SerializeField]
  public UnityEvent onCurrentItemChanged = new UnityEvent();
  [ToggleGroup(nameof(enableEvent))]
  [SerializeField]
  public UnityEvent onItemsChanged = new UnityEvent();

  private void UpdateDeactivateExceptCurrent() {
#if UNITY_EDITOR
    if (UnityEditor.EditorApplication.isPlaying) return;
#endif
    if (items.IsUnset()) return;

    // TODO: if SceneAssetOnly
    if (typeof(T) == typeof(GameObject)) {
      items.ForEach(item => { if (item) (item as GameObject).SetActive(!deactivateExceptCurrent); });
      (CurrentItemOrFirst as GameObject).SetActive(true);
    } else if (typeof(T) == typeof(MonoBehaviour) || typeof(T) == typeof(Component)) {
      items.ForEach(item => { if (item) (item as MonoBehaviour).gameObject.SetActive(!deactivateExceptCurrent); });
      (CurrentItemOrFirst as MonoBehaviour).gameObject.SetActive(true);
    }
  }

  private void OnCurrentItemChanged() {
    UpdateDeactivateExceptCurrent();
    if (enableEvent) onCurrentItemChanged.Invoke();
    // Debug.Log(CurrentItemOrFirst.name);
  }

  public void OnItemsChanged() {
    if (items.IsUnset()) return;
    UpdateDeactivateExceptCurrent();
    if (enableEvent) onItemsChanged.Invoke();
    if (!items.Contains(currentItem)) {
      currentItem = items[0];
    }
  }
  #endregion

  #region HOTKEY
  [ToggleGroup(nameof(enableHotkey), groupTitle: "HOTKEY")]
  public bool enableHotkey = true;

  [ToggleGroup(nameof(enableHotkey))]
  [InfoBox("If using hotkeys, invoke ProcessCollectionInput() of the collection in Update() of this Component.", InfoMessageType.Warning)]
  [ShowInInspector, HideLabel, DisplayAsString]
  private string hotkeyInfoText = "";

  // TODO: Replace by InputModifier
  [ToggleGroup(nameof(enableHotkey))]
  [PropertySpace(SpaceBefore = -20)]
  [HideLabel, Title("Set To Previous Item Key", bold: false, titleAlignment: TitleAlignments.Left)]
  // [LabelText("Set Previous"), LabelWidth(LABEL_WIDTH_KEY)]
  [SerializeField]
  KeyCodeModifier switchPreviousKey = new KeyCodeModifier();

  [ToggleGroup(nameof(enableHotkey))]
  [HideLabel, Title("Set To Next Item Key", bold: false, titleAlignment: TitleAlignments.Left)]
  // [LabelText("Set Next"), LabelWidth(LABEL_WIDTH_KEY)]
  [SerializeField]
  KeyCodeModifier switchNextKey = new KeyCodeModifier();

  [ToggleGroup(nameof(enableHotkey))]
  [HideLabel, Title("Set To Random Item Key", bold: false, titleAlignment: TitleAlignments.Left)]
  // [LabelText("Set Random"), LabelWidth(LABEL_WIDTH_KEY)]
  [SerializeField]
  KeyCodeModifier switchRandomKey = new KeyCodeModifier();

  /// <summary>
  /// Invoke this in Monobehaviour container for quick switching between items using hotkeys
  /// </summary>
  public void ProcessCollectionInput() {
    if (!enableHotkey) return;
    if (switchPreviousKey.IsTriggering) GetAndSetToPreviousItem();
    if (switchNextKey.IsTriggering) GetAndSetToNextItem();
    if (switchRandomKey.IsTriggering) GetAndSetToRandomItem();
  }
  #endregion

  #region BASE GETTERS
  /// <summary>
  /// If index is invalid, return current item
  /// </summary>
  public T GetItemByIndex(int index) {
    throw new NotImplementedException();
  }

  /// <summary>
  /// If current item is not set, set it to the 1st item and return it
  /// </summary>
  public T CurrentItemOrFirst {
    get {
      if (currentItem == null) currentItem = items[0];
      return currentItem;
    }
  }

  public List<T> ItemsNotCurrent {
    get => items.Where(items => items != CurrentItemOrFirst).ToList();
  }

  public T RandomItem {
    get => items.GetRandom();
  }

  public T GetAndSetToRandomItem() {
    if (items.IsUnset()) Debug.Log("not set");
    if (items.IsUnset()) return null;
    currentItem = RandomItem;
    OnCurrentItemChanged();
    return CurrentItemOrFirst;
  }

  public T NextItem {
    get => items.NavNext(currentItem);
  }

  public T GetAndSetToNextItem() {
    if (items.IsUnset()) return null;
    currentItem = NextItem;
    OnCurrentItemChanged();
    return CurrentItemOrFirst;
  }

  public T PreviousItem {
    get => items.NavPrevious(currentItem);
  }

  public T GetAndSetToPreviousItem() {
    if (items.IsUnset()) return null;
    currentItem = PreviousItem;
    OnCurrentItemChanged();
    return CurrentItemOrFirst;
  }
  #endregion

  /// <summary>
  /// Return a (current) item based on Retrieve Mode. In Random Mode, set current item to random. In Iterate Mode, set current item to next one.
  /// </summary>
  public T Retrieve() {
    if (retrieveMode == CollectionRetrieveMode.Random) return GetAndSetToRandomItem();
    if (retrieveMode == CollectionRetrieveMode.Iterate) return GetAndSetToNextItem();
    return CurrentItemOrFirst;
  }
}
