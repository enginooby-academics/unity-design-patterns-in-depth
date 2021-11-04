using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern.Base {
  public class NpcState : State { // ? make abstract to force SetName()
    public enum Name {
      Idle, Patrol, Pursue, Attack, Sleep
    }
    public Name name;
    protected GameObject npc;
    protected Animator animator;
    protected UnityEngine.AI.NavMeshAgent navMeshAgent;
    protected Transform player;
    protected AreaCircular vision;
    protected AreaCircular attackableArea;

    protected bool CanSeePlayer => vision.Contains(player.position);
    protected bool CanAttackPlayer => attackableArea.Contains(player.position);

    public NpcState(GameObject npc, Animator animator, UnityEngine.AI.NavMeshAgent navMeshAgent, Transform player, AreaCircular vision = null, AreaCircular attackableArea = null) {
      this.stage = Stage.Enter;
      this.npc = npc;
      this.animator = animator;
      this.navMeshAgent = navMeshAgent;
      this.player = player;
      this.vision = vision;
      this.attackableArea = attackableArea;
    }
  }
}