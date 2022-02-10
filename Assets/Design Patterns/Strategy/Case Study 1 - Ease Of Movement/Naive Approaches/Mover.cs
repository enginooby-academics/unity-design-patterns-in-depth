using UnityEngine;
using static RayUtils;
using DG.Tweening;

namespace Strategy.Naive {
  public enum EaseMovement { Linear, InBounce, InSine }

  public class Mover : MonoBehaviour {
    [SerializeField]
    private float _speed = 5f;

    [SerializeField]
    private EaseMovement _movementEase;


    void Update() {
      HandleMovement();
    }

    // ! Many conditional statements
    // ! Need to modify this source file if adding new type of eases
    private void HandleMovement() {
      if (MouseButton.Left.IsDown()) {
        Vector3 mousePosOnGround = MousePosOnRayHit.Value.WithY(0);
        switch (_movementEase) {
          case EaseMovement.Linear:
            MoveLinear(mousePosOnGround, _speed);
            break;
          case EaseMovement.InBounce:
            MoveInBounce(mousePosOnGround, _speed);
            break;
          case EaseMovement.InSine:
            MoveInSine(mousePosOnGround, _speed);
            break;
        }
      }
    }

    private void MoveLinear(Vector3 dest, float speed) {
      Debug.Log("Moving with linear ease");
      transform.DOMove(dest, speed).SetSpeedBased(true).SetEase(Ease.Linear);
    }

    private void MoveInBounce(Vector3 dest, float speed) {
      Debug.Log("Moving with in-bounce ease");
      transform.DOMove(dest, speed).SetSpeedBased(true).SetEase(Ease.InBounce);
    }

    private void MoveInSine(Vector3 dest, float speed) {
      Debug.Log("Moving with in-sine ease");
      transform.DOMove(dest, speed).SetSpeedBased(true).SetEase(Ease.InSine);
    }
  }
}
