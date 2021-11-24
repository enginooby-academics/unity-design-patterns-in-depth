using UnityEngine;

public abstract class Attackable : MonoBehaviour {
  protected bool _isDead;
  public bool IsDead => _isDead;

  public abstract void GetAttacked(Attacker attacker, int damage);
  public abstract void Die();
}