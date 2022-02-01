using UnityEngine;
using SCPE;
#if ASSET_QUANTUM_CONSOLE
using QFSW.QC;
#endif

namespace Enginoobz.Graphics {
  public partial class PostFxModifierSCPE : MonoBehaviour {
    private Sketch sketch;
    private Kuwahara kuwahara;
    private EdgeDetection edgeDetection;
    private Mosaic mosaic;

    private bool sketchOriginalState;
    private bool kuwaharaOriginalState;
    private bool edgeDetectionOriginalState;
    private bool mosaicOriginalState;

    private void GetStylizedSettings() {
      Profile.TryGet<Sketch>(out sketch);
      Profile.TryGet<Kuwahara>(out kuwahara);
      Profile.TryGet<EdgeDetection>(out edgeDetection);
      Profile.TryGet<Mosaic>(out mosaic);
    }

    private void GetStylizedOriginalStates() {
      sketchOriginalState = sketch.active;
      kuwaharaOriginalState = kuwahara.active;
      edgeDetectionOriginalState = edgeDetection.active;
      mosaicOriginalState = mosaic.active;
    }

    private void ResetStylizedSettings() {
      sketch.active = sketchOriginalState;
      kuwahara.active = kuwaharaOriginalState;
      edgeDetection.active = edgeDetectionOriginalState;
      mosaic.active = mosaicOriginalState;
    }

    private void RandomizeStylizedSettings() {
      sketch.SetActiveOnRandom();
      kuwahara.SetActiveOnRandom();
      edgeDetection.SetActiveOnRandom();
      mosaic.SetActiveOnRandom();
      if (sketch.active) statesDebug += "sketch ";
      if (kuwahara.active) statesDebug += "kuwahara ";
      if (edgeDetection.active) statesDebug += "edgeDetection ";
      if (mosaic.active) statesDebug += "mosaic ";
    }
#if ASSET_QUANTUM_CONSOLE
    // TODO: Create fallback attribute when asset is not available (could be wrapper?)
    [Command(CommandPrefix.PostFx + "sketch-toggle")]
#endif
    public void ToggleSketch() {
      sketch.active = !sketch.active;
    }

#if ASSET_QUANTUM_CONSOLE
    [Command(CommandPrefix.PostFx + "kuwahara-toggle")]
#endif
    public void ToggleKuwahara() {
      kuwahara.active = !kuwahara.active;
    }

    public void ActivateKuwahara(bool isActive) {
      kuwahara.active = isActive;
    }

#if ASSET_QUANTUM_CONSOLE
    [Command(CommandPrefix.PostFx + "edge-detection-toggle")]
#endif
    public void ToggleEdgeDetection() {
      edgeDetection.active = !edgeDetection.active;
    }

#if ASSET_QUANTUM_CONSOLE
    [Command(CommandPrefix.PostFx + "mosaic-toggle")]
#endif
    public void ToggleMosaic() {
      mosaic.active = !mosaic.active;
    }
  }
}