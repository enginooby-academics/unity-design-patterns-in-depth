using UnityEngine;

namespace Flyweight {
  public class Boid : MonoBehaviour {
    public enum Specie { Flamingo, Shorebird, Starling }
    public enum EndangeredStatus { CR, EN, VU }
    public enum Gender { Male, Female }

    protected int _mass;
    protected Gender _gender;

    protected BoidsController controller;

    private Vector3 _separationForce;
    private Vector3 _cohesionForce;
    private Vector3 _alignmentForce;
    private Vector3 _avoidWallsForce;
    private Vector3 _velocity;

    private void Start() {
      controller = FindObjectOfType<BoidsController>();
      SetUniqueData();
    }

    private void SetUniqueData() {
      _mass = Random.Range(50, 100);
      _gender = (Random.Range(0f, 1f) < 0.5f) ? Gender.Male : Gender.Female;
    }

    private void OnMouseDown() {
      DisplayInfo();
    }

    protected virtual void DisplayInfo() {
      InfoPanel.Instance.massLabel.text = _mass.ToString() + "g";
      InfoPanel.Instance.genderLabel.text = _gender.ToString();
    }

    private void Update() {
      Fly();
    }

    private void Fly() {
      CalculateForces();
      CalculateVelocity();
      transform.position += _velocity * Time.deltaTime;
      transform.rotation = Quaternion.LookRotation(_velocity);
    }

    private void CalculateForces() {
      Vector3 seperationSum = Vector3.zero;
      Vector3 positionSum = Vector3.zero;
      Vector3 headingSum = Vector3.zero;

      int boidsNearby = 0;

      for (int i = 0; i < controller.Boids.Count; i++) {

        if (this != controller.Boids[i]) {

          Vector3 otherBoidPosition = controller.Boids[i].transform.position;
          float distToOtherBoid = (transform.position - otherBoidPosition).magnitude;

          if (distToOtherBoid < controller.boidPerceptionRadius) {

            seperationSum += -(otherBoidPosition - transform.position) * (1f / Mathf.Max(distToOtherBoid, .0001f));
            positionSum += otherBoidPosition;
            headingSum += controller.Boids[i].transform.forward;

            boidsNearby++;
          }
        }
      }

      if (boidsNearby > 0) {
        _separationForce = seperationSum / boidsNearby;
        _cohesionForce = (positionSum / boidsNearby) - transform.position;
        _alignmentForce = headingSum / boidsNearby;
      } else {
        _separationForce = Vector3.zero;
        _cohesionForce = Vector3.zero;
        _alignmentForce = Vector3.zero;
      }

      if (GetMinDistToBorder(transform.position, controller.cageSize) < controller.avoidWallsTurnDist) {
        // Back to center of cage
        _avoidWallsForce = -transform.position.normalized;
      } else {
        _avoidWallsForce = Vector3.zero;
      }
    }

    private void CalculateVelocity() {
      Vector3 force =
          _separationForce * controller.separationWeight +
          _cohesionForce * controller.cohesionWeight +
          _alignmentForce * controller.alignmentWeight +
          _avoidWallsForce * controller.avoidWallsWeight;

      _velocity = transform.forward * controller.boidSpeed;
      _velocity += force * Time.deltaTime;
      _velocity = _velocity.normalized * controller.boidSpeed;

    }

    private float GetMinDistToBorder(Vector3 pos, float cageSize) {
      float halfCageSize = cageSize / 2f;
      return Mathf.Min(Mathf.Min(
          halfCageSize - Mathf.Abs(pos.x),
          halfCageSize - Mathf.Abs(pos.y)),
          halfCageSize - Mathf.Abs(pos.z)
      );
    }
  }
}
