using UnityEngine;
using Sirenix.OdinInspector;

// TODO:
// + Timespan for component/gameobject
// + Alternative fast Update()

/// <summary>
/// * Common convenient public functions & extenstion methods (useful esp. in binding events w/o writing more code) for all custom Components (MonoBehaviours).
/// * vs. ComponentOperator base is for built-in Unity Components
/// </summary>
public abstract class MonoBehaviourBase : MonoBehaviour {
  // [SerializeField, HideInInspector]
  // private Vector3? _initalPosition = null;
  // public Vector3 InitialPosition => _initalPosition ??= transform.position;

  [FoldoutGroup("MonoBehaviour Common")]
  // [Button]
  public void GetAutoReferences() {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.ExecuteMenuItem("Tools/AutoRefs/Set AutoRefs");
#endif
  }

  #region ACTIVITY ===================================================================================================================================
  [FoldoutGroup("MonoBehaviour Common")]
  [SerializeField, Min(0f)] float lifespan;

  public void DisableForSecs(float seconds) => this.Disable(seconds);


  public void ToggleActive() {

  }
  #endregion ===================================================================================================================================

  #region EVENT ===================================================================================================================================
  #endregion ===================================================================================================================================
}
