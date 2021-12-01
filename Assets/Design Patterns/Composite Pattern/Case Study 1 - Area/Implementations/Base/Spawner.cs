using Sirenix.OdinInspector;
using UnityEngine;
using Drawing;

namespace CompositePattern.Case1.Base {
  public class Spawner : MonoBehaviourGizmos {
    [SerializeField, SerializeReference, HideLabel]
    private Area _spawnArea = new AreaComposite();

    private void Reset() {
      (_spawnArea as AreaComposite)
        .Add(new AreaAxis(gameObject))
        .Add(new AreaCircular(gameObject))
        .Add(new AreaAxis(gameObject));
    }

    public override void DrawGizmos() {
      _spawnArea?.DrawGizmos();
    }

    [Button]
    public void SpawnRandom() {
      GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
      obj.transform.position = _spawnArea.RandomPoint;
    }
  }
}
