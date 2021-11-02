using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Digger
{
    // ensure class initializer is called whenever scripts recompile
    [InitializeOnLoad]
    public class PlayModeStateChanged
    {
        // register an event handler when the class is initialized
        static PlayModeStateChanged()
        {
            SceneManager.sceneLoaded += SceneLoaded;
            EditorApplication.playModeStateChanged += LogPlayModeState;
        }

        private static void LogPlayModeState(PlayModeStateChange state)
        {
            switch (state) {
                case PlayModeStateChange.EnteredEditMode:
                {
                    for (var i = 0; i < SceneManager.sceneCount; ++i) {
                        var scene = SceneManager.GetSceneAt(i);
                        if (scene.IsValid() && scene.isLoaded) {
                            DiggerMasterEditor.LoadAllChunks(scene);
                        }
                    }

                    break;
                }
                case PlayModeStateChange.ExitingEditMode:
                {
                    NativeCollectionsPool.Instance.Dispose();
                    
                    for (var i = 0; i < SceneManager.sceneCount; ++i) {
                        var scene = SceneManager.GetSceneAt(i);
                        if (scene.IsValid() && scene.isLoaded) {
                            DiggerMasterEditor.OnEnterPlayMode(scene);
                        }
                    }

                    break;
                }
                case PlayModeStateChange.ExitingPlayMode:
                    NativeCollectionsPool.Instance.Dispose();
                    break;
            }
        }

        private static void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (Application.isPlaying) {
                DiggerMasterEditor.OnEnterPlayMode(scene);
            }
        }
    }
}