using UnityEngine;

namespace Decorator.Base {
  /// <summary>
  /// Base class for all bullet enhancement to extend from.
  /// </summary>
  public abstract class BulletDecorator<TSelfReferenceType> : MonoBehaviour, IBullet {
    protected IBullet _bullet;
    // public GameObject BulletGameObject => (_bullet as MonoBehaviour).gameObject;
    // public BulletDriver BulletBase => BulletGameObject.GetComponent<BulletDriver>();

    public BulletDecorator(IBullet bullet) {
      _bullet = bullet;
      // BulletGameObject.AddComponent(typeof(TSelfReferenceType));
    }

    public virtual float Damage => _bullet.Damage;

    public virtual float Speed => _bullet.Speed;

    public virtual void Hit(AttackTarget target) {
      _bullet.Hit(target);
    }
  }
}
