using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

namespace Singleton {
  public class SceneLoader : MonoBehaviour {
    private void Awake() {
      DontDestroyOnLoad(this);
    }

    [Button]
    public void ReloadScene() {
      SceneUtils.ReloadScene();
    }

    [Button]
    public void LoadAnotherScene() {
      SceneManager.LoadScene("1 Singleton B");
    }

    [Button]
    public void LoadAnotherSceneAdditively() {
      SceneManager.LoadScene("1 Singleton B", LoadSceneMode.Additive);
    }
  }
}