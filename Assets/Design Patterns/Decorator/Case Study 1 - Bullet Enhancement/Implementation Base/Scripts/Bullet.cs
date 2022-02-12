using UnityEngine;

namespace Decorator.Base {
  public class Bullet : MonoBehaviour, IBullet {
    // ? No need MonoBehaviour
    [SerializeField] private float _damage = 1;

    [SerializeField] private float _speed = 5;

    public float Damage => _damage;

    public float Speed => _speed;

    public void Hit(AttackTarget target) {
      target.GetHit(_damage);
    }
  }
}