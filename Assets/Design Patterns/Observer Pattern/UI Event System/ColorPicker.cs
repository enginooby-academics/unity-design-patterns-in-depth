using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems; // !
using UnityEngine.UI; // !
using UnityEngine.UI.ProceduralImage; // !

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
        var image = swatch.gameObject.GetComponent<ProceduralImage>();
        if (!image) return;
        onSwatchSelected.Invoke(image.color);
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
