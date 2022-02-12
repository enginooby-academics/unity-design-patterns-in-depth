using UnityEngine;

namespace Decorator.Base {
  /// <summary>
  ///   Base class for all bullet enhancements to extend from.
  /// </summary>
  public abstract class BulletDecorator<TSelfReferenceType> : MonoBehaviour, IBullet {
    protected IBullet _bullet;

    public BulletDecorator(IBullet bullet) => _bullet = bullet;

    public virtual float Damage => _bullet.Damage;

    public virtual float Speed => _bullet.Speed;

    public virtual void Hit(AttackTarget target) {
      _bullet.Hit(target);
    }
  }
}