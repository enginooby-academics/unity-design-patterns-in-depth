using UnityEngine;
using SCPE;
using QFSW.QC;

namespace Enginoobz.Graphics {
  public partial class PostFxModifierSCPE : MonoBehaviour {
    private Colorize colorize;
    private HueShift3D hueShift3D;

    private bool colorizeOriginalState;
    private bool hueShift3DOriginalState;

    private void GetImageOriginalStates() {
      colorizeOriginalState = colorize.active;
      hueShift3DOriginalState = hueShift3D.active;
    }

    private void ResetImageSettings() {
      colorize.active = colorizeOriginalState;
      hueShift3D.active = hueShift3DOriginalState;
    }

    private void RandomizeImageSettings() {
      colorize.SetActiveOnRandom(.2f);
      hueShift3D.SetActiveOnRandom(.2f);
      if (colorize.active) statesDebug += "colorize ";
      if (hueShift3D.active) statesDebug += "hueShift3D ";
    }

    private void GetImageSettings() {
      Profile.TryGet<Colorize>(out colorize);
      Profile.TryGet<HueShift3D>(out hueShift3D);
    }

    [Command(CommandPrefix.PostFx + "colorize-toggle")]
    public void ToggleColorize() {
      colorize.active = !colorize.active;
    }

    [Command(CommandPrefix.PostFx + "hue-shift-toggle")]
    public void ToggleHueShift3D() {
      hueShift3D.active = !hueShift3D.active;
    }
  }
}