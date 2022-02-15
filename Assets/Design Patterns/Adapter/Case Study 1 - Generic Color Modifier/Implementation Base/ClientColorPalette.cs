using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

namespace AdapterPattern.Case1.Base1 {
  /// <summary>
  ///   Centralization fo color modifiers.
  /// </summary>
  public class ClientColorPalette : MonoBehaviour {
    // + global color w/ randomize
    // + randomize all

    [SerializeField] [InlineEditor] private List<ClientColorModifier> _modifiers = new List<ClientColorModifier>();

    [Button]
    public void RetrieveAllModifiers() {
      // + add only valid modifier
      // + filter by concrete IColorizable type
      _modifiers = FindObjectsOfType<ClientColorModifier>().ToList();
    }
  }
}