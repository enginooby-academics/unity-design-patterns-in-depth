using UnityEngine;
using SCPE;
using QFSW.QC;

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

    [Command(CommandPrefix.PostFx + "sketch-toggle")]
    public void ToggleSketch() {
      sketch.active = !sketch.active;
    }

    [Command(CommandPrefix.PostFx + "kuwahara-toggle")]
    public void ToggleKuwahara() {
      kuwahara.active = !kuwahara.active;
    }

    public void ActivateKuwahara(bool isActive) {
      kuwahara.active = isActive;
    }

    [Command(CommandPrefix.PostFx + "edge-detection-toggle")]
    public void ToggleEdgeDetection() {
      edgeDetection.active = !edgeDetection.active;
    }

    [Command(CommandPrefix.PostFx + "mosaic-toggle")]
    public void ToggleMosaic() {
      mosaic.active = !mosaic.active;
    }
  }
}