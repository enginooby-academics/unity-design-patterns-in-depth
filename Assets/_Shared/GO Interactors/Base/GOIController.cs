using Sirenix.OdinInspector;
using UnityEngine;

// TODO: Implement hover

/// <summary>
/// General controller for all GameObject Interactors.
/// </summary>
public class GOIController : MonoBehaviourSingleton<GOIController> {
  // TIP: Dont destroy general singleton if there is any specific singleton
  protected override bool DoesInstanceExist => _instance && !_instance.GetType().IsSubclassOf(typeof(GOIController));

  [PropertyOrder(-1)]
  [SerializeField, ShowIf(nameof(_isGeneral)), OnValueChanged(nameof(GetInteractorByType))]
  private ReferenceConcreteType<GOI> _interactorType;
  [PropertyOrder(-1)]
  [SerializeField, InlineEditor, ShowIf(nameof(_isGeneral))]
  protected GOI _interactor;
  [SerializeField, InlineEditor]
  private Selector _selector;
  [SerializeField]
  private GOIControlKeyPreset _keyPreset;
  [SerializeField, HideInInspector]
  protected bool _isGeneral = true;


  // public override void AwakeSingleton() => GetDependencies();

  private void Start() {
    _selector = _selector ?? FindObjectOfType<Selector>();
    GetInteractor();
  }

  private void GetInteractorByType() {
    if (_interactorType != null)
      _interactor = (GOI)FindObjectOfType(_interactorType.Value);
  }

  protected virtual void GetInteractor() {
    _interactor = _interactor ?? FindObjectOfType<GOI>();
    if (_interactorType != null)
      _interactor = (GOI)FindObjectOfType(_interactorType.Value) ?? _interactorType.CreateInstance();
  }

  protected virtual void Update() {
    ProcessSelected();
    ProcessInteracted();
    ProcessInteractingByRayCast();
    ProcessRevertingByRayCast();
    ProcessSwitchingEffect();
  }

  private void ProcessSwitchingEffect() {
    if (_keyPreset.IncrementEffectKey.IsTriggering) {
      _interactor.IncrementEffect();
    }

    if (_keyPreset.DecrementEffectKey.IsTriggering) {
      _interactor.DecrementEffect();
    }
  }

  // REFACTOR
  private void ProcessInteractingByRayCast() {
    if (!_keyPreset.InteractKey.IsTriggering || !RayUtils.IsMouseRayHit) return;

    var hits = RayUtils.HitsFromMouseRay;
    if (_keyPreset.EnablePassThrough) {
      foreach (var hit in hits) {
        _interactor.Interact(hit.transform.gameObject);
      }
    } else {
      _interactor.Interact(hits[0].transform.gameObject);
    }
  }

  private void ProcessRevertingByRayCast() {
    if (!_keyPreset.RevertKey.IsTriggering || !RayUtils.IsMouseRayHit) return;

    var hits = RayUtils.HitsFromMouseRay;
    if (_keyPreset.EnablePassThrough) {
      foreach (var hit in hits) {
        _interactor.InteractRevert(hit.transform.gameObject);
      }
    } else {
      _interactor.InteractRevert(hits[0].transform.gameObject);
    }
  }

  // TODO: Revert, Toggle
  private void ProcessSelected() {
    if (_keyPreset.InteractSelectedKey.IsTriggering && _selector)
      _interactor.Interact(_selector.CurrentSelectedObject);
  }

  private void ProcessInteracted() {
    if (_keyPreset.InteractInteractedKey.IsTriggering) {
      _interactor.InteractForInteracted();
    }

    if (_keyPreset.RevertInteractedKey.IsTriggering) {
      _interactor.InteractRevertForInteracted();
    }

    if (_keyPreset.RestoreInteractedKey.IsTriggering) {
      _interactor.InteractRestoreForInteracted();
    }
  }
}

/// <summary>
/// Controller constrainted to a specific GameObject Interactor type.
/// </summary>
public abstract class GOIController<TSelf, TInteractor> : GOIController
where TSelf : GOIController<TSelf, TInteractor>
where TInteractor : GOI<TInteractor> {
  [PropertyOrder(-1)]
  [SerializeField, InlineEditor, LabelText("Interactor")]
  private TInteractor _tInteractor;

  protected override bool DoesInstanceExist => _instance && _instance.GetType() == typeof(TSelf);

  protected override void GetInteractor() {
    // TIP: Provide fallback if MonoBehaviourSingleton component is not available in scene.
    // _interactor = _interactor ?? FindObjectOfType<T>() ?? MonoBehaviourSingleton<T>.Instance; // Get singleton instance in subclass from generic .
    // _interactor = _interactor ?? FindObjectOfType<T>() ?? GOInteractor<T>.Instance;
    _tInteractor = _tInteractor ?? FindObjectOfType<TInteractor>() ?? GOI<TInteractor>.Instance;
    _interactor = _tInteractor;
  }

  // REFACTOR: replaced field in child class
  // TIP: Relace and hide a field in parent by specific type in child class (GOInteracor => GOInteractor<T>)
  protected void Reset() {
    _isGeneral = false;
  }
}