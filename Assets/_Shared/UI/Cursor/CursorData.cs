using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

namespace Enginoobz.UI {
  [CreateAssetMenu(fileName = "CS_", menuName = "UI/Cusor Data", order = 0)]
  public class CursorData : ScriptableObject {
    [SerializeField] private CursorName _name;

    [SerializeField] [OnValueChanged(nameof(CentralizeHotSpot))]
    private Texture2D _texture;

    [SerializeField] [InlineButton(nameof(CentralizeHotSpot), "Center")]
    private Vector2 _hotSpot;

    public void CentralizeHotSpot() {
      _hotSpot = new Vector2(_texture.width / 2, _texture.height / 2);
    }

    public bool CompareName(CursorName nameTarget) => _name == nameTarget;

    public void SetCursor() {
      if (!_texture) return;

      Cursor.SetCursor(_texture, _hotSpot, CursorMode.Auto);
      // Debug.Log("Set Cursor");
    }
  }

  // TIP: Explicitly assign int to enum used in SO, to prevent swapping value when change enum order
  public enum CursorName {
    None = 0,
    Move = 1,
    Attack = 2,
    UI = 3,
    Pickup = 4
  }
}