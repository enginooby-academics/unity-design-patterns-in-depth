// * Usage: invoke Select() or assign tag and add collider on target

using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// TODO: implement on right mouse click, layers, disable all tags/layers, enable all tags/layers, toggle/unselect mode
public class Selector : MonoBehaviour {
  public static Selector Instance;
  enum SelectEffect { Highlight, Animate }
  enum SelectMode { Single, Multiple }


  [InfoBox("Single mode: only one object can be selected at one time")]
  [EnumToggleButtons]
  [SerializeField]
  private SelectMode selectMode = SelectMode.Single;

  [SerializeField] private bool canToggleSelection = true;

  [EnumToggleButtons]
  [SerializeField]
  private SelectEffect selectEffect = SelectEffect.Highlight;

  [ShowIf(nameof(selectEffect), SelectEffect.Highlight)]
  [SerializeField]
  private Highlighter highlighter;

  #region FILTERS - Decide which object is selectable based on tags, layers, events
  [Header("ON LEFT MOUSE CLICK (OLMC)")]
  [SerializeField]
  [Tooltip("Excluding Unselectable Tags OLMC")]
  private bool allTagsSelectableOlmc = true;
  [SerializeField] private List<string> selectableTagsOlmc = new List<string>();
  [SerializeField] private List<string> unselectableTagsOlmc = new List<string>();

  [Header("ON MOUSE HOVER (OMH)")]
  [SerializeField]
  [Tooltip("Excluding Unselectable Tags OMH")]
  private bool allTagsSelectableOmh = true;
  [SerializeField] private List<string> selectableTagsOmh = new List<string>();
  [SerializeField] private List<string> unselectableTagsOmh = new List<string>();
  #endregion

  public GameObject CurrentSelectedObject { get; private set; }
  private Camera mainCamera;
  private Ray ray;
  private RaycastHit hit;

  private void Awake() {
    Instance = this;
    mainCamera = Camera.main;
    highlighter ??= FindObjectOfType<Highlighter>();
  }

  void Update() {
    if (selectableTagsOlmc.Count > 0 || allTagsSelectableOlmc) ProcessOLMC();
    if (selectableTagsOmh.Count > 0 || allTagsSelectableOmh) ProcessOMH();
  }

  private void ProcessOLMC() {
    if (MouseButton.Left.IsDown()) {
      ray = mainCamera.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out hit) && IsTagSelectableOlmc(hit.collider.tag)) {
        Toggle(hit.collider.gameObject);
      }
    }
  }

  private void ProcessOMH() {
    ray = mainCamera.ScreenPointToRay(Input.mousePosition);
    if (Physics.Raycast(ray, out hit) && IsTagSelectableOmh(hit.collider.tag)) {
      if (!CurrentSelectedObject || CurrentSelectedObject != hit.collider.gameObject) {
        Toggle(hit.collider.gameObject);
      }
    }
  }

  private void Toggle(GameObject target) {
    if (target == CurrentSelectedObject) {
      if (canToggleSelection) Deselect(target);
    } else {
      Select(target);
    }

    // TODO: Implement the same for Multiple Selection mode
  }

  private bool IsTagSelectableOlmc(string tag) {
    if (unselectableTagsOlmc.Contains(tag)) return false;
    if (selectableTagsOlmc.Contains(tag) || allTagsSelectableOlmc) return true;
    else return false;
  }

  private bool IsTagSelectableOmh(string tag) {
    if (unselectableTagsOmh.Contains(tag)) return false;
    if (selectableTagsOmh.Contains(tag) || allTagsSelectableOmh) return true;
    else return false;
  }

  private void Select(GameObject target) {
    // $"Select {target.name}".Log();
    switch (selectEffect) {
      case SelectEffect.Highlight:
      default:
        highlighter.Highlight(target);
        break;
    }

    if (selectMode == SelectMode.Single && CurrentSelectedObject) {
      Deselect(CurrentSelectedObject);
    }
    CurrentSelectedObject = target;
  }

  private void Deselect(GameObject target) {
    // $"Deselect {target.name}".Log();
    switch (selectEffect) {
      case SelectEffect.Highlight:
      default:
        highlighter.Unhighlight(target);
        break;
    }
    CurrentSelectedObject = null;
  }
}
