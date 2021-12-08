using UnityEngine;
using DG.Tweening;

namespace SubclassSandboxPattern.Case1.Base {
  /// <summary>
  /// * [The 'Sandbox' base class] 
  /// Couple with ProceduralUtils, VFXs, TransformOperator component, DOTween asset
  /// </summary>
  public abstract class Builder : MonoBehaviour {
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
    protected GameObject AddCube(Vector3 localPos, float size = 1f, string name = "Primitive") {
      GameObject primitive = new GameObject(name);
      MeshFilter meshFilter = primitive.AddComponent<MeshFilter>();
      MeshRenderer meshRenderer = primitive.AddComponent<MeshRenderer>();

      meshFilter.mesh = ProceduralUtils.CreateCubeMesh(size);
      meshRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
      meshRenderer.material.color = Color.red;
      primitive.transform.SetPos(localPos + transform.position);
      primitive.transform.SetParent(transform);

      return primitive;
    }

    protected void AddSphere() {

    }

    protected void AddCylinder() {

    }

    protected void ShakePosition() {
      transform.DOShakePosition(1f).SetLoops(-1);
    }

    protected void ShakeRotation() {
      transform.DOShakeRotation(1f).SetLoops(-1);
    }

    protected void ShakeScale() {
      transform.DOShakeScale(1f).SetLoops(-1);
    }
  }
}
