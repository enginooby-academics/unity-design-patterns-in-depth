using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern.Base {
  public class NpcPatrolState : NpcState {
    GameObject currentCheckpoint;

    public NpcPatrolState(GameObject npc, Animator animator, UnityEngine.AI.NavMeshAgent navMeshAgent, Transform player, AreaCircular vision = null, AreaCircular attackableArea = null)
    : base(npc, animator, navMeshAgent, player, vision, attackableArea) {
      name = NpcState.Name.Patrol;
      navMeshAgent.speed = 2;
      navMeshAgent.isStopped = false;
    }

    public override void Enter() {
      currentCheckpoint = Environment.Instance.Checkpoints.GetNearestTo(npc.transform.position);
      currentCheckpoint = Environment.Instance.Checkpoints.GetPrevious(currentCheckpoint);
      animator.SetTrigger("isWalking");
      base.Enter();
    }

    public override void Update() {
      if (CanSeePlayer) {
        incommingState = new NpcPursueState(npc, animator, navMeshAgent, player, vision, attackableArea);
        stage = Stage.Exit;
      }
      if (navMeshAgent.remainingDistance < 1) {
        MoveToNextCheckpoint();
      }
    }

    private void MoveToNextCheckpoint() {
      currentCheckpoint = Environment.Instance.Checkpoints.GetNext(currentCheckpoint);
      Debug.Log("Current Checkpoint: " + currentCheckpoint.name);
      navMeshAgent.SetDestination(currentCheckpoint.transform.position);
    }

    public override void Exit() {
      animator.ResetTrigger("isWalking");
      base.Exit();
    }
  }
}