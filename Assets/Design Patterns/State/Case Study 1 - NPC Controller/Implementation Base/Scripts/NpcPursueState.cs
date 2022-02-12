using UnityEngine;
using UnityEngine.AI;

namespace StatePattern.Base {
  public class NpcPursueState : NpcState {
    public NpcPursueState(GameObject npc, Animator animator, NavMeshAgent navMeshAgent, Transform player,
      AreaCircular vision = null, AreaCircular attackableArea = null)
      : base(npc, animator, navMeshAgent, player, vision, attackableArea) {
      name = Name.Pursue;
      navMeshAgent.speed = 5;
      navMeshAgent.isStopped = false;
    }

    public override void Enter() {
      animator.SetTrigger("isRunning");
      base.Enter();
    }

    public override void Update() {
      Debug.Log("Pursuing");
      navMeshAgent.SetDestination(player.position);
      if (navMeshAgent.hasPath) {
        if (CanAttackPlayer) {
          incommingState = new NpcAttackState(npc, animator, navMeshAgent, player, vision, attackableArea);
          stage = Stage.Exit;
        }
        else if (!CanSeePlayer) {
          incommingState = new NpcPatrolState(npc, animator, navMeshAgent, player, vision, attackableArea);
          stage = Stage.Exit;
        }
      }
    }

    public override void Exit() {
      animator.ResetTrigger("isRunning");
      base.Exit();
    }
  }
}