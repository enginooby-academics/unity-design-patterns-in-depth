// * Collection of convenient public method involving GameObject (tag, name...) to bind in event listener
using UnityEngine;

public class GameObjectOperator : MonoBehaviour {
  public void SetTag(string tag) => gameObject.tag = tag;
  public void SetTagUntagged() => SetTag("Untagged");
  public void SetName(string name) => gameObject.name = name;

  public void DeactivateForSecs(float seconds) {
  }
}
