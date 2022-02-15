using UnityEngine;
#if ASSET_DOTWEEN
using DG.Tweening;
#endif

namespace SubclassSandboxPattern.Case1.Base {
  /// <summary>
  ///   * [The 'Sandbox' base class]
  ///   Couple with ProceduralUtils, VFXs, TransformOperator component, DOTween asset
  /// </summary>
  public abstract class Builder : MonoBehaviourGizmos {
    // Subsystems
    // load vfxes from resources

    private void Start() => Build();

    protected abstract void Build();

    protected void Rebuild() {
      transform.DestroyChildren();
      Build();
    }

    /// <summary>
    ///   Create primitive shape with the given local position.
    /// </summary>
    protected GameObject AddCube(Vector3 localPos, float size = 1f, string name = "Primitive", Color? color = null) {
      var primitive = new GameObject(name);
      var meshFilter = primitive.AddComponent<MeshFilter>();
      var meshRenderer = primitive.AddComponent<MeshRenderer>();

      meshFilter.mesh = ProceduralUtils.CreateCubeMesh(size);
      meshRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
      meshRenderer.material.color = color ?? Color.red;
      primitive.transform.SetPos(localPos + transform.position);
      primitive.transform.SetParent(transform);

      return primitive;
    }

    protected void AddSphere() { }

    protected void AddCylinder() { }

    protected void ShakePosition(GameObject go) {
#if ASSET_DOTWEEN
      go.transform.DOShakePosition(1f).SetLoops(-1);
#endif
    }

    protected void ShakeRotation(GameObject go) {
#if ASSET_DOTWEEN
      go.transform.DOShakeRotation(1f).SetLoops(-1);
#endif
    }

    protected void ShakeScale(GameObject go) {
#if ASSET_DOTWEEN
      go.transform.DOShakeScale(1f).SetLoops(-1);
#endif
    }
  }
}