using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginooby.Attribute;
using Enginooby.Core;
#endif

#if ASSET_DOTWEEN
using DG.Tweening;
#endif

namespace IteratorPattern.Case1.Base {
  public class Traveller : SerializedMonoBehaviour {
    [SerializeField] [OnValueChanged(nameof(CreateIterator))]
    private IIterableContainer<Waypoint> _path = new();

    [SerializeField] [SerializeReference]
    private IIterator<Waypoint> _iterator; // ! not serialized, has to init in Start()

    private void Start() {
      CreateIterator();
    }


    // IMPL: Reflection for iterator types
    // [ValueDropdown(nameof(_iteratorTypes)), SerializeField, SerializeReference, OnValueChanged(nameof(CreateIterator))]
    // private Type _iteratorType;

    // [SerializeReference, HideInInspector]
    // private List<Type> _iteratorTypes = new List<Type>();

    // private void Reset() {
    //   _iteratorTypes = TypeUtils.GetTypesOf<IIterator<Waypoint>>();
    // }

    private void CreateIterator() {
      if (_path.Result == null) return;
      // _iterator = _path.GetIterator(_iteratorType);
      // _iterator = _path.GetIterator(typeof(IteratorFilterColor));
      // _iterator = _path.GetIterator<IteratorFilterColor>();
      _iterator = _path.Result.GetIterator();
    }

    [Button]
    public void MoveOnPath() {
#if ASSET_DOTWEEN
      if (_iterator.HasNext())
        transform.DOMove(_iterator.GetNext().transform.position, 2)
          .OnComplete(() => MoveOnPath());
      ;
#endif
    }
  }
}