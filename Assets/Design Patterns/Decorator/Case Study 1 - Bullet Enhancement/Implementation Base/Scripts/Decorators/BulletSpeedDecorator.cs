namespace Decorator.Base {
  /// <summary>
  ///   Add more damage & speed. Has wind VFX on launch.
  /// </summary>
  public class BulletSpeedDecorator : BulletDecorator<BulletSpeedDecorator> {
    private readonly float _bonusSpeed;

    public BulletSpeedDecorator(IBullet bullet, float bonusSpeed = 5f) : base(bullet) => _bonusSpeed = bonusSpeed;

    public override float Speed => base.Speed + _bonusSpeed;

    public override void Hit(AttackTarget target) {
      base.Hit(target);
    }
  }
}