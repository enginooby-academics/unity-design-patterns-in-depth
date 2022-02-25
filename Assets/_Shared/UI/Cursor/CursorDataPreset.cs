using System.Collections.Generic;
using UnityEngine;

namespace Enginooby.UI {
  // [SOVariant]
  // Collection of cursors in a certain style (e.g. pixel, modern), used in a certain game theme (e.g. RPG, racing).
  [CreateAssetMenu(fileName = "CSPreset_", menuName = "UI/Cusor Data Preset", order = 0)]
  public class CursorDataPreset : ScriptableObject {
    [SerializeField] private List<CursorData> _cursorDatas;

    private CursorData _currentCursor; // dirty flag

    // ! Not work. Must invoke in MonoBehaviour's Awake/Start() instead.
    private void Awake() {
      Init();
    }

    /// <summary>
    ///   Set to the first cursor in the preset.
    /// </summary>
    public void Init() {
      // Debug.Log("Init cursor");
      _currentCursor = _cursorDatas[0];
      _currentCursor.SetCursor();
    }

    /// <summary>
    ///   [Safe-Update method]
    ///   Change cursor if given cursor name is different than current's.
    ///   If not found in the preset, set to the first cursor.
    /// </summary>
    public void Set(CursorName cursorName) {
      if (!_currentCursor.CompareName(cursorName)) {
        _currentCursor = _cursorDatas.Find(cursor => cursor.CompareName(cursorName)) ?? _cursorDatas[0];
        _currentCursor.SetCursor();
      }
    }
  }
}