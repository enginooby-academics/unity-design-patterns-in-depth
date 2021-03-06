using UnityEngine;
using UnityEngine.AI;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace StatePattern.Base {
  public class NpcAI : MonoBehaviour {
    // TIP: private component (of this gameObject) attribute architecture - make private SerializeField, with public getter property
    // TIP: components are check null and get in Start(), this way, attributes are optionally serialized
    [SerializeField] private Animator _animator;

    [SerializeField] private NavMeshAgent _navMeshAgent;

    public Transform player;

    [SerializeField] [HideLabel] private AreaCircular vision = new("Vision", angle: 30f);

    [SerializeField] [HideLabel] private AreaCircular attackableArea = new("Attack Area", angle: 30f, radius: 7f);

    private State currentState;

    public Animator Animator => _animator;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;

    private void Reset() {
      vision.SetGameObject(gameObject);
      attackableArea.SetGameObject(gameObject);
    }

    private void Start() {
      // _animator ??= GetComponent<Animator>();
      // _navMeshAgent ??= GetComponent<NavMeshAgent>();
      _animator = GetComponent<Animator>();
      _navMeshAgent = GetComponent<NavMeshAgent>();
      currentState = new NpcIdleState(gameObject, _animator, _navMeshAgent, player, vision, attackableArea);
    }

    private void Update() {
      currentState = currentState.Process();
    }

    private void OnDrawGizmos() {
      vision.DrawGizmos();
      attackableArea.DrawGizmos(Color.red);
    }
  }
}