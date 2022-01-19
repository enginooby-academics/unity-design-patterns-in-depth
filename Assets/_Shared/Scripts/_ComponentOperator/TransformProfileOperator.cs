using Sirenix.OdinInspector;
using UnityEngine;

public class TransformProfileOperator : ComponentOperator<Transform> {
  [SerializeField, InlineEditor]
  private TransformProfile _transformProfile;

  private void Start() {
    _transformProfile?.InitTranslation(transform);
  }

  private void LateUpdate() {
    _transformProfile?.LateUpdate(transform);
  }

  public void Stop() {
    _transformProfile?.Stop();
  }

  void OnDrawGizmosSelected() {
    _transformProfile?.OnDrawGizmosSelected(gameObject);
  }
}