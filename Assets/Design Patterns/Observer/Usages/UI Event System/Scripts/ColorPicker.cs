using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems; // !
using UnityEngine.UI; // !

namespace ObserverPattern.UIEventSystem {
  public class ColorPicker : MonoBehaviour {
    public UnityEvent<Color> onSwatchSelected = new UnityEvent<Color>();
    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    void Start() {
      raycaster = GetComponent<GraphicRaycaster>();
      eventSystem = GetComponent<EventSystem>();
    }

    void Update() {
      if (MouseButton.Left.IsDown()) HandleSelectSwatch();
    }

    private void HandleSelectSwatch() {
      foreach (RaycastResult swatch in GetSwatchesUnderMouse()) {
#if ASSET_PROCEDURAL_IMAGE
        var image = swatch.gameObject.GetComponent<UnityEngine.UI.ProceduralImage.ProceduralImage>();
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
