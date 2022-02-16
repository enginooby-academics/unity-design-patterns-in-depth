using UnityEngine;

namespace UpdateMethodPattern.Case1.Unity2 {
  // Attach to all object that should have a custom Update (and Start) method 
  // The parent class will handle the registration of the OnUpdate method
  public class MovingShape : RegisteredUpdateBehaviour {
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _mapRadius = 10f;

    protected override void OnStart() => SetRandomDirection();

    private void SetRandomDirection() => transform.SetRotation(0, Random.Range(0, 360), 0);

    public override void OnUpdate(float deltaTime) => WanderInsideCircle(deltaTime);

    private void WanderInsideCircle(float deltaTime) {
      var forwardPos = transform.position + transform.forward * (_speed * deltaTime);

      // move forward while inside circle otherwise change direction
      if (forwardPos.IsInsideCircle(_mapRadius))
        transform.position = forwardPos;
      else SetRandomDirection();
    }
  }
}