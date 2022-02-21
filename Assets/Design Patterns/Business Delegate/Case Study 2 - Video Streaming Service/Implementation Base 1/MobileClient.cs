using UnityEngine;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace BusinessDelegatePattern.Case2.Base1 {
  public class MobileClient : MonoBehaviour {
    [SerializeField] private BusinessDelegate _businessDelegate;

    [ValueDropdown(nameof(Movies))] [SerializeField]
    private string _movie;

    private IEnumerable<string> Movies => BusinessDelegate.GetAllSupportedMovies();

    [Button]
    public void PlaybackMovie() => _businessDelegate.PlaybackMovie(_movie);
  }
}