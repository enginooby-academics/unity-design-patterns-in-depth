namespace Decorator.Base {
  /// <summary>
  ///   Inflict burning damage overtime. Has fire VFX on launch & burn VFX on hit.
  /// </summary>
  public class BulletFireDecorator : BulletDecorator<BulletFireDecorator> {
    public BulletFireDecorator(IBullet bullet) : base(bullet) {
    }

    public override void Hit(AttackTarget target) {
      base.Hit(target);
      BurnTarget(target);
    }

    private void BurnTarget(AttackTarget target) {
      target.GetBurned();
    }
  }
}