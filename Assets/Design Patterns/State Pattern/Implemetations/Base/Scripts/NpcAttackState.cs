using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace StatePattern.Base {
  public class NpcAttackState : NpcState {
    float rotationSpeed = 2f;
    AudioSource shootAudio;

    public NpcAttackState(GameObject npc, Animator animator, UnityEngine.AI.NavMeshAgent navMeshAgent, Transform player, AreaCircular vision = null, AreaCircular attackableArea = null)
    : base(npc, animator, navMeshAgent, player, vision, attackableArea) {
      name = NpcState.Name.Attack;
      shootAudio = npc.GetComponent<AudioSource>();
    }

    public override void Enter() {
      animator.SetTrigger("isShooting");
      navMeshAgent.isStopped = true;
      shootAudio.Play();
      base.Enter();
    }

    public override void Update() {
      Debug.Log("Attacking");
      npc.transform.DOLookAt(player.position, rotationSpeed, AxisConstraint.Y);

      if (!CanAttackPlayer) {
        incommingState = new NpcIdleState(npc, animator, navMeshAgent, player, vision, attackableArea);
        stage = Stage.Exit;
      }
    }

    public override void Exit() {
      animator.ResetTrigger("isShooting");
      shootAudio.Stop();
      base.Exit();
    }
  }
}