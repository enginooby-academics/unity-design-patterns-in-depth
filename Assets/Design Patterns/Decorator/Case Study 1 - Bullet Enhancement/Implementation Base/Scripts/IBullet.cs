namespace Decorator.Base {
  public interface IBullet {
    float Damage { get; }

    float Speed { get; }

    void Hit(AttackTarget target);
  }
}