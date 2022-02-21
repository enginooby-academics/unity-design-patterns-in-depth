/// <summary>
///   Collection of convenient public method involving GameObject (tag, name...) to bind in event listener.
/// </summary>
public class GameObjectOperator : MonoBehaviourBase {
  public void SetTag(string newTag) => gameObject.tag = newTag;

  public void SetTagUntagged() => SetTag("Untagged");

  public void SetName(string newName) => gameObject.name = newName;

  public void DeactivateForSecs(float seconds) { }
}