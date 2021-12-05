namespace MediatorPattern.Case1.Base {
  /// <summary>
  /// * A 'Concrete Mediator' implementation. 
  /// </summary>
  public class CollisionResolverAlike : ICollisionResolver {
    public void ResolveCollision(Shape shape1, Shape shape2) {
      if (shape1.GetType() == shape2.GetType()) {
        shape1.gameObject.InvertTranslationalDirection();
      }
    }
  }
}
