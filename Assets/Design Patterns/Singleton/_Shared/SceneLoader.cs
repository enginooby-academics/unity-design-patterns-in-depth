#if ASM
#if ODIN_INSPECTOR
using AdvancedSceneManager.Models;
using Sirenix.OdinInspector;
using UnityEngine;
#else
using Enginoobz.Attribute;
#endif

namespace SingletonPattern {
  public class SceneLoader : MonoBehaviour {
    [SerializeField]
    [LabelWidth(80)]
    [InlineButton(nameof(LoadScene2), "Load")]
    [InlineButton(nameof(LoadScene2Additively), "Load Additively")]
    private Scene _scene2;

    [Button]
    public void ReloadScene() => SceneUtils.ReloadScene();

    public void LoadScene2() => _scene2.OpenSingle();

    public void LoadScene2Additively() => _scene2.Open();
  }
}
#endif