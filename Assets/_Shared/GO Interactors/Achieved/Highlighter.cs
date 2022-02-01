#if URP_OUTLINE
using UnityEngine;
using EPOOutline;
using Sirenix.OdinInspector;
using System.Collections.Generic;
// using QFSW.QC;

public class Highlighter : MonoBehaviour {
  // TODO: Replace by Collection
  [ValueDropdown(nameof(highlightVfxPrefabs))]
  [SerializeField]
  private Outlinable currentHighlightVfx;
  [SerializeField]
  private List<Outlinable> highlightVfxPrefabs;

  public void Highlight(GameObject go) {
    var outlinable = go.GetComponent<Outlinable>();
    if (outlinable) {
      outlinable.enabled = true;
      return;
    }

    // instantiate a new Outlinable to use as template instead of directly using SerializeField prefab 
    // to prevent linking (update in GameObject will cause update in prefab)
    Outlinable templateInstance = Instantiate(currentHighlightVfx, Vector3.zero, Quaternion.identity);
    var outlinableToUse = go.AddComponent<Outlinable>().GetLinkedCopyOf(templateInstance);
    Destroy(templateInstance.gameObject);

    var outlineTarget = new OutlineTarget(go.GetComponent<Renderer>());
    outlinableToUse.OutlineTargets.Add(outlineTarget);
  }

  public void Highlight(GameObject[] gos) {
    foreach (var go in gos) {
      Highlight(go);
    }
  }

  public void Unhighlight(GameObject target) {
    var outlinable = target.GetComponent<Outlinable>();
    if (!outlinable) return;

    // CONSIDER: just disable to reuse for performance, but need update if change effect in the runtime
    outlinable.enabled = false;
    // Destroy(outlinable);
  }

  // [Command(CommandPrefix.Highlight + "tag")]
  public void HighlightByTag(string tag) {
    Highlight(GameObject.FindGameObjectsWithTag(tag));
  }

  //! PROBLEM: this does not copy all deep parameters related to Fill Parameter
  private void CopyOutlinable(ref Outlinable outlinable, Outlinable template) {
    outlinable.RenderStyle = template.RenderStyle;
    outlinable.DrawingMode = template.DrawingMode;

    outlinable.OutlineLayer = template.OutlineLayer;
    outlinable.OutlineParameters.FillPass.Shader = template.OutlineParameters.FillPass.Shader;
    outlinable.OutlineParameters.FillPass.SetColor("_PublicColor", template.OutlineParameters.FillPass.GetColor("_PublicColor"));
    outlinable.OutlineParameters.Color = template.OutlineParameters.Color;
    outlinable.OutlineParameters.BlurShift = template.OutlineParameters.BlurShift;
    outlinable.OutlineParameters.DilateShift = outlinable.OutlineParameters.DilateShift;
    outlinable.OutlineParameters.Enabled = outlinable.OutlineParameters.Enabled;

    outlinable.FrontParameters.Color = template.FrontParameters.Color;
    outlinable.FrontParameters.FillPass.Shader = template.FrontParameters.FillPass.Shader;
    outlinable.FrontParameters.BlurShift = template.FrontParameters.BlurShift;
    outlinable.FrontParameters.BlurShift = template.FrontParameters.BlurShift;
    outlinable.FrontParameters.BlurShift = template.FrontParameters.BlurShift;
    outlinable.FrontParameters.FillPass.SetColor("_PublicColor", template.FrontParameters.FillPass.GetColor("_PublicColor"));

    outlinable.BackParameters.Color = template.BackParameters.Color;
    outlinable.BackParameters.FillPass.Shader = template.BackParameters.FillPass.Shader;
    outlinable.BackParameters.BlurShift = template.BackParameters.BlurShift;
    outlinable.BackParameters.BlurShift = template.BackParameters.BlurShift;
    outlinable.BackParameters.BlurShift = template.BackParameters.BlurShift;
    outlinable.BackParameters.FillPass.SetColor("_PublicColor", template.BackParameters.FillPass.GetColor("_PublicColor"));
  }
}
#endif
