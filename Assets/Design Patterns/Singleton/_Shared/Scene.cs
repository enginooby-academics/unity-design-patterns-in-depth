using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginooby.Attribute;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace SingletonPattern {
  [Serializable]
  [InlineProperty]
  public class Scene {
    [field: SerializeField] [HideLabel] public Object Value { private set; get; }

    public void Load() => SceneManager.LoadScene(Value.name);

    public void LoadAdditively() => SceneManager.LoadScene(Value.name, LoadSceneMode.Additive);
  }
}