using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

namespace FacadePattern.Case1.Base {
  /// <summary>
  ///   The 'Client' class.
  ///   Makes use of the 'Facade' class to manipulate the overall graphics.
  /// </summary>
  public class GraphicsClient : MonoBehaviour {
    [Button]
    public void MakeGraphicsHorror() {
      GraphicsController.Instance.SetupHorror();
    }

    [Button]
    public void MakeGraphicsCinematic() {
      GraphicsController.Instance.SetupCinematic();
    }

    [Button]
    public void MakeGraphicsArtistic() {
      GraphicsController.Instance.SetupArtistic();
    }
  }
}