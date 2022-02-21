using UnityEngine;
#if ODIN_INSPECTOR

#else
using Enginooby.Attribute;
#endif

public class TransformProfileOperator : ComponentOperator<Transform> {
  [SerializeField] private TransformProfile _transformProfile;

  private void Start() {
    _transformProfile?.InitTranslation(transform);
  }

  private void LateUpdate() {
    _transformProfile?.OnLateUpdate(transform);
  }

  private void OnDrawGizmosSelected() {
    _transformProfile?.OnDrawGizmosSelected(gameObject);
  }

  public void Stop() {
    _transformProfile?.Stop();
  }
}