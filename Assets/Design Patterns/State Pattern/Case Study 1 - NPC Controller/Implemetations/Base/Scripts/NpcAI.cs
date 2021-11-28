using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

namespace StatePattern.Base {
  public class NpcAI : MonoBehaviour {
    // TIP: private component (of this gameObject) attribute architecture - make private SerializeField, with public getter property
    // TIP: components are check null and get in Start(), this way, attributes are optionally serialized
    [SerializeField] private Animator _animator;
    public Animator Animator => _animator;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public Transform player;

    [SerializeField, HideLabel] AreaCircular vision = new AreaCircular(label: "Vision", angle: 30f);
    [SerializeField, HideLabel] AreaCircular attackableArea = new AreaCircular(label: "Attack Area", angle: 30f, radius: 7f);
    State currentState;

    private void Reset() {
      vision.SetComponentOwner(gameObject);
      attackableArea.SetComponentOwner(gameObject);
    }

    void Start() {
      // _animator ??= GetComponent<Animator>();
      // _navMeshAgent ??= GetComponent<NavMeshAgent>();
      _animator = GetComponent<Animator>();
      _navMeshAgent = GetComponent<NavMeshAgent>();
      currentState = new NpcIdleState(gameObject, _animator, _navMeshAgent, player, vision, attackableArea);
    }

    void Update() {
      currentState = currentState.Process();
    }

    private void OnDrawGizmos() {
      vision.DrawGizmos();
      attackableArea.DrawGizmos(color: Color.red);
    }
  }
}
