// * Use case: Camera following w/ offset, enemy chasing
// * Alternative: Cinematic, NavMesh

using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

// ? CONSIDER: Separate classes for 2 methods if they don't communicate
[RequireComponent(typeof(Rigidbody))]
public class Follower : MonoBehaviour {
  [SerializeField] [EnumToggleButtons] private Method method = Method.Instant;
  [SerializeField] [HideLabel] private Reference target;
  [SerializeField] private bool enableDetectArea;

  [ShowIf(nameof(enableDetectArea))] [BoxGroup("Detect Area")] [SerializeField] [HideLabel]
  private Area detectArea;


  private void Start() {
    if (method == Method.Towards) rb = this.AddRigidBodyIfNotExist();
  }

  private void Update() {
    if (enableDetectArea && !detectArea.Contains(transform.position)) return;
    if (method == Method.Towards) FollowByRigidBody();
  }

  // TIP: Set position should be in LateUpdate();
  private void LateUpdate() {
    if (!target.GameObject) return;
    if (enableDetectArea && !detectArea.Contains(transform.position)) return;


    if (method == Method.Instant) FollowBySettingPosition();
  }

  private void OnDrawGizmos() {
    if (enableDetectArea) detectArea.DrawGizmos();
  }

  private enum Method {
    Instant,
    Towards,
  }

  #region METHOD INSTANT ===================================================================================================================================

  [ShowIf(nameof(method), Method.Instant)]
  [OnValueChanged(nameof(OnOffsetUpdate))]
  [InlineButton(nameof(CalcOffsetFromCurrentPos), "Current")]
  [SerializeField]
  private Vector3 offset = new(50, 0, 0);

  private void CalcOffsetFromCurrentPos() {
    if (!target.GameObject) return;

    offset = target.GameObject.transform.position - transform.position;
  }

  [SerializeField] [EnumToggleButtons] private AxisFlag frozenPosition = AxisFlag.Y;

  [SerializeField] [MinMaxSlider(-100, 100, true)]
  private Vector2 yBoundaryWorld;

  private Vector3 dest;

  private void OnOffsetUpdate() {
    if (!target.GameObject) return;
    transform.position = target.GameObject.transform.position + offset;
  }

  private void FollowBySettingPosition() {
    dest = target.GameObject.transform.position + offset;
    if (frozenPosition.HasFlag(AxisFlag.X)) dest.x = transform.position.x;
    if (frozenPosition.HasFlag(AxisFlag.Y)) dest.y = transform.position.y;
    if (frozenPosition.HasFlag(AxisFlag.Z)) dest.z = transform.position.z;
    ProcessBoundary();
    transform.position = dest;
  }

  private void ProcessBoundary() {
    // dest.y = yBoundaryWorld.Clamp(dest.y);
    // IMPL
  }

  #endregion ===================================================================================================================================

  #region METHOD TOWARDS ===================================================================================================================================

  [ShowIf(nameof(method), Method.Towards)] [SerializeField]
  private float speed = 5f;

  private Rigidbody rb;

  private void FollowByRigidBody() {
    var lookDirection = (target.GameObject.transform.position - transform.position).normalized;
    rb.AddForce(lookDirection * speed);
  }

  #endregion ===================================================================================================================================
}