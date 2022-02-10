using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern.Base {
  public class NpcIdleState : NpcState {
    public NpcIdleState(GameObject npc, Animator animator, UnityEngine.AI.NavMeshAgent navMeshAgent, Transform player, AreaCircular vision = null, AreaCircular attackableArea = null)
    : base(npc, animator, navMeshAgent, player, vision, attackableArea) {
      name = NpcState.Name.Idle;
    }

    public override void Enter() {
      animator.SetTrigger("isIdle");
      base.Enter();
    }

    public override void Update() {
      if (CanSeePlayer) {
        incommingState = new NpcPursueState(npc, animator, navMeshAgent, player, vision, attackableArea);
        stage = Stage.Exit;
      } else if (10.Percent()) {
        incommingState = new NpcPatrolState(npc, animator, navMeshAgent, player, vision, attackableArea);
        stage = Stage.Exit;
      }
    }

    public override void Exit() {
      animator.ResetTrigger("isIdle");
      base.Exit();
    }
  }
}