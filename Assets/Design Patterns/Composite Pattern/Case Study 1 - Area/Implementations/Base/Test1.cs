using Sirenix.OdinInspector;
using UnityEngine;

namespace CompositePattern.Case1.Base {
  public class Test1 : MonoBehaviour {
    [SerializeField, SerializeReference, HideLabel]
    private Area _spawnArea = new AreaComposite();

    private void Reset() {
      (_spawnArea as AreaComposite).Add(new AreaAxis(gameObject));
      (_spawnArea as AreaComposite).Add(new AreaCircular(gameObject));
      (_spawnArea as AreaComposite).Add(new AreaAxis(gameObject));
    }

    private void OnDrawGizmos() {
      _spawnArea?.DrawGizmos();
    }

    [Button]
    public void SpawnRandom() {
      GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
      obj.transform.position = _spawnArea.RandomPoint;
    }
  }
}
