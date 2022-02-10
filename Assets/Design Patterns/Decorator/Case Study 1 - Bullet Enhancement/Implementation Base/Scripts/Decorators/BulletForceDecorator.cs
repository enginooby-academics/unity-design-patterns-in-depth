namespace Decorator.Base {
  /// <summary>
  /// Add more damage. Increase bullet size. Add force to target on hit.
  /// </summary>
  public class BulletForceDecorator : BulletDecorator<BulletForceDecorator> {
    private float _bonusDamage;

    public BulletForceDecorator(IBullet bullet, float bonusDamage = 1f) : base(bullet) {
      _bonusDamage = bonusDamage;
    }

    public override float Damage => base.Damage + _bonusDamage;

    public override void Hit(AttackTarget target) {
      base.Hit(target);
      target.GetHit(_bonusDamage);
      Push(target);
    }

    private void Push(AttackTarget target) {
      print($"Bullet pushed {target.name}.");
      target.transform.MoveZ(10);
    }
  }
}
