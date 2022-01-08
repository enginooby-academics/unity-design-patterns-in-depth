using UnityEngine;
using DG.Tweening;

namespace CommandPattern.Case1.Naive1 {
  // TODO: Shared with Base1
  public class Cube : MonoBehaviour {
    public void MoveX(float amount) {
      float newX = transform.position.x + amount;
      transform.DOMoveX(newX, .5f);
    }

    public void MoveY(float amount) {
      float newY = transform.position.y + amount;
      transform.DOMoveY(newY, .5f);
    }
  }
}
