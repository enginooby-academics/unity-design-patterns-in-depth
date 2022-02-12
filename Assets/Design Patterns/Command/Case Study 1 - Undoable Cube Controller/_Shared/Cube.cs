using UnityEngine;

namespace CommandPattern.Case1 {
  /// <summary>
  ///   * A 'Receiver' class
  ///   Should define sufficient methods to implement all commands.
  /// </summary>
  public class Cube : MonoBehaviour {
    public void MoveX(float amount) {
      var newX = transform.position.x + amount;
      transform.MoveX(newX, .5f);
    }

    public void MoveY(float amount) {
      var newY = transform.position.y + amount;
      transform.MoveY(newY, .5f);
    }
  }
}