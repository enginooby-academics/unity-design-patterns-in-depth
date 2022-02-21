using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

// TODO:
// + Timespan for component/gameobject
// + Alternative fast Update()
// + Create script template of this script

/// <summary>
///   * Common convenient public functions & extenstion methods (useful esp. in binding events w/o writing more code) for
///   all custom Components (MonoBehaviours).
///   * vs. ComponentOperator base is for built-in Unity Components
/// </summary>
public abstract class MonoBehaviourBase : MonoBehaviour {
  protected virtual void Awake() { }

  protected virtual void Start() { }

  protected virtual void Update() { }

  protected virtual void FixedUpdate() { }

  protected virtual void LateUpdate() { }

  // TIP: Best practice using static variable: cache old value and restore it after operation
  private void OnDrawGizmos() {
    var oldColor = Gizmos.color;
    DrawGizmos();
    Gizmos.color = oldColor;
  }

  private void OnDrawGizmosSelected() {
    var oldColor = Gizmos.color;
    DrawGizmosOnSelected();
    Gizmos.color = oldColor;
  }

  protected virtual void DrawGizmos() { }

  protected virtual void DrawGizmosOnSelected() { }

  // [SerializeField, HideInInspector]
  // private Vector3? _initalPosition = null;
  // public Vector3 InitialPosition => _initalPosition ??= transform.position;

  [FoldoutGroup("MonoBehaviour Common")]
  // [Button]
  public void GetAutoReferences() {
#if UNITY_EDITOR
    EditorApplication.ExecuteMenuItem("Tools/AutoRefs/Set AutoRefs");
#endif
  }

  #region LAZY LOCAL COMPONENT CACHE

  // Alternative to CacheStaticUtils
  // ? Does it cost many memories if variables are not used
  // ? Use a dictionary (like in CacheStaticUtils) instead of separate variables
  // Prefer to component with only one of its type on the GO ([DisallowedMultipleComponent])
  private readonly Dictionary<Type, Component> _cachedComponents = new();

  // private Rigidbody _rigidbody = null;
  // public Rigidbody Rigidbody => _rigidbody ??= GetComponent<Rigidbody>();

  // Cons: expose un-used/un-available components from a MonoBehaviour => use protected instead of public
  public Transform Transform => My<Transform>();

  protected Vector3 _position {
    get => Transform.position;
    set => Transform.position = value;
  }

  protected Quaternion _rotation {
    get => Transform.rotation;
    set => Transform.rotation = value;
  }

  protected Rigidbody _rigidbody => My<Rigidbody>();
  protected Collider _collider => My<Collider>();
  protected BoxCollider _boxCollider => My<BoxCollider>();
  protected MeshRenderer _meshRenderer => My<MeshRenderer>();
  protected MeshFilter _meshFilter => My<MeshFilter>();
  protected Animator _animator => My<Animator>();

  /// <summary>
  ///   Get cached singleton (on a GO) component.
  /// </summary>
  public T My<T>() where T : Component {
    // TODO: https://stackoverflow.com/questions/16580912/optimizing-dictionary-trygetvalue?answertab=votes#tab-top
    if (_cachedComponents.TryGetValue(typeof(T), out var cachedComponent)) // print("Get cached component");
      return (T) cachedComponent;

    if (TryGetComponent<T>(out var component)) {
      // print("Get uncached component");
      _cachedComponents.Add(typeof(T), component);
      return component;
    }

    // ? Shoud add component if not found
    return null;
  }

  #endregion

  #region ACTIVITY =====================================================================================================

  [FoldoutGroup("MonoBehaviour Common")] [SerializeField] [Min(0f)]
  private float lifespan;

  public void DisableTemporarily<T>(float durationInSec) where T : MonoBehaviour {
    // TODO
    // My<T>().enabled = false;
  }

  public void DisableColliderForSecs(float seconds) {
    _collider.enabled = false;
    Invoke(nameof(EnableCollider), seconds);
  }

  protected void EnableCollider() => _collider.enabled = true;

  public void DisableForSecs(float seconds) => this.Disable(seconds);


  public void ToggleActive() { }

  /// <summary>
  ///   Instantiate at this component's position with Quaternion identity.
  /// </summary>
  protected new T Instantiate<T>(T prefab) where T : Component =>
    Instantiate(prefab, transform.position, Quaternion.identity);

  /// <summary>
  ///   Instantiate at this component's position with Quaternion identity. <br />
  ///   After the given lifespan, the spawned GameObject/component is destroyed.
  /// </summary>
  protected T Instantiate<T>(T prefab, float lifespan, bool destroyGameObject = true) where T : Component {
    var component = Instantiate(prefab);
    if (destroyGameObject)
      Destroy(component.gameObject, lifespan);
    else
      Destroy(component, lifespan);
    return component;
  }

  #endregion ===========================================================================================================

  #region EVENT ========================================================================================================

  #endregion ===========================================================================================================
}