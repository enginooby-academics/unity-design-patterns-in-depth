using UnityEngine;
#if ASSET_NAUGHTY_ATTRIBUTES
using NaughtyAttributes;
#else
using Enginooby.Attribute;
#endif

namespace Techniques.Referencing {
  public class SerializeTagFinder : MonoBehaviour {
    [SerializeField] [Tag] private string _targetTag;

    private void Start() {
      var target = GameObject.FindGameObjectWithTag(_targetTag);
      Debug.Log(target is null ? "Target not found." : "Target found.", this);
    }
  }
}