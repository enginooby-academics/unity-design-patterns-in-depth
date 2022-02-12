#if ASSET_ALINE
public class MonoBehaviourGizmos : Drawing.MonoBehaviourGizmos {
}
#else
public class MonoBehaviourGizmos : UnityEngine.MonoBehaviour {
  private void OnDrawGizmos() {
    DrawGizmos();
  }

  public virtual void DrawGizmos() {
  }
}
#endif