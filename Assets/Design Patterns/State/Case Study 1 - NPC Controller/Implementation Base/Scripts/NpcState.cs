using UnityEngine;
using UnityEngine.AI;

namespace StatePattern.Base {
  public class NpcState : State {
    // ? make abstract to force SetName()
    public enum Name {
      Idle,
      Patrol,
      Pursue,
      Attack,
      Sleep
    }

    protected Animator animator;
    protected AreaCircular attackableArea;
    public Name name;
    protected NavMeshAgent navMeshAgent;
    protected GameObject npc;
    protected Transform player;
    protected AreaCircular vision;

    public NpcState(GameObject npc, Animator animator, NavMeshAgent navMeshAgent, Transform player,
      AreaCircular vision = null, AreaCircular attackableArea = null) {
      stage = Stage.Enter;
      this.npc = npc;
      this.animator = animator;
      this.navMeshAgent = navMeshAgent;
      this.player = player;
      this.vision = vision;
      this.attackableArea = attackableArea;
    }

    protected bool CanSeePlayer => vision.Contains(player.position);
    protected bool CanAttackPlayer => attackableArea.Contains(player.position);
  }
}