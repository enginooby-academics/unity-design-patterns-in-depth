namespace MediatorPattern.Case1.Base {
  /// <summary>
  ///   * The 'Mediator Constract'
  /// </summary>
  public interface ICollisionResolver {
    void ResolveCollision(Shape shape1, Shape shape2);
  }
}