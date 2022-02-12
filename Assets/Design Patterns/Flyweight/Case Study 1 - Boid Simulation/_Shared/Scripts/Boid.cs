using UnityEngine;

namespace Flyweight {
  public class Boid : MonoBehaviour {
    public enum EndangeredStatus {
      CR,
      EN,
      VU
    }

    public enum Gender {
      Male,
      Female
    }

    public enum Specie {
      Flamingo,
      Shorebird,
      Starling
    }

    private Vector3 _alignmentForce;
    private Vector3 _avoidWallsForce;
    private Vector3 _cohesionForce;
    protected Gender _gender;

    protected int _mass;

    private Vector3 _separationForce;
    private Vector3 _velocity;

    protected BoidsController controller;

    private void Start() {
      controller = FindObjectOfType<BoidsController>();
      SetUniqueData();
    }

    private void Update() => Fly();
    private void OnMouseDown() => DisplayInfo();

    private void SetUniqueData() {
      _mass = Random.Range(50, 100);
      _gender = Random.Range(0f, 1f) < 0.5f ? Gender.Male : Gender.Female;
    }

    protected virtual void DisplayInfo() {
      InfoPanel.Instance.massLabel.text = _mass + "g";
      InfoPanel.Instance.genderLabel.text = _gender.ToString();
    }

    private void Fly() {
      CalculateForces();
      CalculateVelocity();
      transform.position += _velocity * Time.deltaTime;
      transform.rotation = Quaternion.LookRotation(_velocity);
    }

    private void CalculateForces() {
      var seperationSum = Vector3.zero;
      var positionSum = Vector3.zero;
      var headingSum = Vector3.zero;

      var boidsNearby = 0;

      for (var i = 0; i < controller.Boids.Count; i++)
        if (this != controller.Boids[i]) {
          var otherBoidPosition = controller.Boids[i].transform.position;
          var distToOtherBoid = (transform.position - otherBoidPosition).magnitude;

          if (distToOtherBoid < controller.boidPerceptionRadius) {
            seperationSum += -(otherBoidPosition - transform.position) * (1f / Mathf.Max(distToOtherBoid, .0001f));
            positionSum += otherBoidPosition;
            headingSum += controller.Boids[i].transform.forward;

            boidsNearby++;
          }
        }

      if (boidsNearby > 0) {
        _separationForce = seperationSum / boidsNearby;
        _cohesionForce = positionSum / boidsNearby - transform.position;
        _alignmentForce = headingSum / boidsNearby;
      }
      else {
        _separationForce = Vector3.zero;
        _cohesionForce = Vector3.zero;
        _alignmentForce = Vector3.zero;
      }

      if (GetMinDistToBorder(transform.position, controller.cageSize) <
          controller.avoidWallsTurnDist) // Back to center of cage
        _avoidWallsForce = -transform.position.normalized;
      else
        _avoidWallsForce = Vector3.zero;
    }

    private void CalculateVelocity() {
      var force =
        _separationForce * controller.separationWeight +
        _cohesionForce * controller.cohesionWeight +
        _alignmentForce * controller.alignmentWeight +
        _avoidWallsForce * controller.avoidWallsWeight;

      _velocity = transform.forward * controller.boidSpeed;
      _velocity += force * Time.deltaTime;
      _velocity = _velocity.normalized * controller.boidSpeed;
    }

    private float GetMinDistToBorder(Vector3 pos, float cageSize) {
      var halfCageSize = cageSize / 2f;
      return Mathf.Min(Mathf.Min(
          halfCageSize - Mathf.Abs(pos.x),
          halfCageSize - Mathf.Abs(pos.y)),
        halfCageSize - Mathf.Abs(pos.z)
      );
    }
  }
}