using UnityEngine;
using UnityEngine.AI;
#if ASSET_DOTWEEN
using DG.Tweening;
#endif

namespace StatePattern.Base {
  public class NpcAttackState : NpcState {
    private readonly float rotationSpeed = 2f;
    private readonly AudioSource shootAudio;

    public NpcAttackState(GameObject npc, Animator animator, NavMeshAgent navMeshAgent, Transform player,
      AreaCircular vision = null, AreaCircular attackableArea = null)
      : base(npc, animator, navMeshAgent, player, vision, attackableArea) {
      name = Name.Attack;
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
#if ASSET_DOTWEEN
      npc.transform.DOLookAt(player.position, rotationSpeed, AxisConstraint.Y);
#endif

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