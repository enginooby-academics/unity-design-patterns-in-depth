#if ASM
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using UnityEngine;
using AdvancedSceneManager.Models;

namespace SingletonPattern {
  public class SceneLoader : MonoBehaviour {
    [SerializeField, LabelWidth(80)]
    [InlineButton(nameof(LoadScene2), label: "Load")]
    [InlineButton(nameof(LoadScene2Additively), label: "Load Additively")]
    private Scene _scene2;

    [Button]
    public void ReloadScene() => SceneUtils.ReloadScene();

    public void LoadScene2() => _scene2.OpenSingle();

    public void LoadScene2Additively() => _scene2.Open();
  }
}
#endif