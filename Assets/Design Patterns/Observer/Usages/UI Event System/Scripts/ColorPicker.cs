using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;
// !

// !

namespace ObserverPattern.UIEventSystem {
  public class ColorPicker : MonoBehaviour {
    public UnityEvent<Color> onSwatchSelected = new UnityEvent<Color>();
    private EventSystem eventSystem;
    private PointerEventData pointerEventData;
    private GraphicRaycaster raycaster;

    private void Start() {
      raycaster = GetComponent<GraphicRaycaster>();
      eventSystem = GetComponent<EventSystem>();
    }

    private void Update() {
      if (MouseButton.Left.IsDown()) HandleSelectSwatch();
    }

    private void HandleSelectSwatch() {
      foreach (var swatch in GetSwatchesUnderMouse()) {
#if ASSET_PROCEDURAL_IMAGE
        var image = swatch.gameObject.GetComponent<ProceduralImage>();
        if (!image) return;
        onSwatchSelected.Invoke(image.color);
#endif
      }
    }

    private List<RaycastResult> GetSwatchesUnderMouse() {
      pointerEventData = new PointerEventData(eventSystem);
      pointerEventData.position = Input.mousePosition;

      var results = new List<RaycastResult>();
      raycaster.Raycast(pointerEventData, results);
      return results;
    }
  }
}