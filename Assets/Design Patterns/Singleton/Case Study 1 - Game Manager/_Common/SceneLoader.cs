using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

namespace Singleton {
  public class SceneLoader : MonoBehaviour {
    public static SceneLoader Instance;

    private void Awake() {
      if (Instance) {
        Destroy(gameObject);
      } else {
        Instance = this;
        DontDestroyOnLoad(gameObject);
      }
    }

    [Button]
    public void ReloadScene() {
      SceneUtils.ReloadScene();
    }

    [Button]
    public void LoadAnotherScene() {
      SceneManager.LoadScene("Scene 2");
    }

    [Button]
    public void LoadAnotherSceneAdditively() {
      SceneManager.LoadScene("Scene 2", LoadSceneMode.Additive);
    }
  }
}