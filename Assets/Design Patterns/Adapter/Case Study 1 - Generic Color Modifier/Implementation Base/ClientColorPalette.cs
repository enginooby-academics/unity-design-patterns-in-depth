using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AdapterPattern.Case1.Base1 {
  /// <summary>
  /// Centralization fo color modifiers.
  /// </summary>
  public class ClientColorPalette : MonoBehaviour {
    // + global color w/ randomize
    // + randomize all

    [SerializeField, InlineEditor]
    private List<ClientColorModifier> _modifiers = new List<ClientColorModifier>();

    [Button]
    public void RetrieveAllModifiers() {
      // + add only valid modifier
      // + filter by concrete IColorizable type
      _modifiers = FindObjectsOfType<ClientColorModifier>().ToList();
    }
  }
}