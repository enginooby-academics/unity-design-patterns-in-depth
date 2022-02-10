using UnityEngine;
#if ASSET_DOTWEEN
using DG.Tweening;
#endif

namespace SubclassSandboxPattern.Case1.Base {
  /// <summary>
  /// * [The 'Sandbox' base class] 
  /// Couple with ProceduralUtils, VFXs, TransformOperator component, DOTween asset
  /// </summary>
  public abstract class Builder : MonoBehaviourGizmos {
    // Subsystems
    // load vfxs from resources

    void Start() {
      Build();
    }

    protected abstract void Build();

    protected void Rebuild() {
      transform.DestroyChildren();
      Build();
    }

    /// <summary>
    /// Create primitive shape with the given local position.
    /// </summary>
    protected GameObject AddCube(Vector3 localPos, float size = 1f, string name = "Primitive", Color? color = null) {
      GameObject primitive = new GameObject(name);
      MeshFilter meshFilter = primitive.AddComponent<MeshFilter>();
      MeshRenderer meshRenderer = primitive.AddComponent<MeshRenderer>();

      meshFilter.mesh = ProceduralUtils.CreateCubeMesh(size);
      meshRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
      meshRenderer.material.color = color ?? Color.red;
      primitive.transform.SetPos(localPos + transform.position);
      primitive.transform.SetParent(transform);

      return primitive;
    }

    protected void AddSphere() {

    }

    protected void AddCylinder() {

    }

    protected void ShakePosition(GameObject gameObject) {
#if ASSET_DOTWEEN
      gameObject.transform.DOShakePosition(1f).SetLoops(-1);
#endif
    }

    protected void ShakeRotation(GameObject gameObject) {
#if ASSET_DOTWEEN
      gameObject.transform.DOShakeRotation(1f).SetLoops(-1);
#endif
    }

    protected void ShakeScale(GameObject gameObject) {
#if ASSET_DOTWEEN
      gameObject.transform.DOShakeScale(1f).SetLoops(-1);
#endif
    }
  }
}
