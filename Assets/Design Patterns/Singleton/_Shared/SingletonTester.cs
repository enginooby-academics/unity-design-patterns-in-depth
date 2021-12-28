using UnityEngine;
using Sirenix.OdinInspector;
using AdvancedSceneManager.Models;
using System.Collections;
using static UnityEngine.SceneManagement.SceneManager;
using System.Linq;

namespace SingletonPattern {
  /// <summary>
  /// For testing singleton regarding: uniqueness (old is persistent, new is destroyed), persistance, laziness.
  /// ! Replay after each test.
  /// </summary>
  public class SingletonTester : MonoBehaviourSingleton<SingletonTester> {
    [SerializeField]
    [InfoBox("ASM scene can be found in Settings/Resources with the same name of the scene. Scene also needs added in the Build setting.")]
    private Scene _scene2;

    [SerializeField]
    private GameObject _singleton;

    private string singletonName;

    private void Start() {
      singletonName = _singleton.name;
    }

    [Button]
    [LabelText("In Current Scene")]
    [BoxGroup("Uniqueness Tests")]
    public void TestUniquenessOnCurrentScene() {
      Instantiate(_singleton);
      // ! add a small delay wait for new instance get destroyed
      Invoke(nameof(TestUniqueness), .2f);
    }

    [Button]
    [LabelText("On Scene 2 Additively Loaded")]
    [BoxGroup("Uniqueness Tests")]
    public void TestUniquenessOnLoadScene2Additively() {
      // ! temperory solution: add a small delay to ensure Testuniqueness is performed after new scene is opened.
      Invoke(nameof(TestUniqueness), .2f);
      _scene2.Open();
    }

    private void TestUniqueness() {
      var singletons = Resources.FindObjectsOfTypeAll<GameObject>()
                              .Where(go => go.name.Contains(singletonName))
                              .ToArray();

      if (singletons.Length > 1) {
        print("Failed");
      } else {
        print("Passed");
      }
    }

    [Button]
    [LabelText("On Current Scene Reloaded")]
    [BoxGroup("Persistance Tests")]
    public void TestPersistanceOnReloadCurrentScene() {
      StartCoroutine(TestPersistanceOnReloadCurrentSceneCoroutine());
    }

    public IEnumerator TestPersistanceOnReloadCurrentSceneCoroutine() {
      var operation = LoadSceneAsync(GetActiveScene().name);
      while (!operation.isDone) yield return null;
      TestPersistance();
    }

    [Button]
    [LabelText("On Scene 2 Loaded")]
    [BoxGroup("Persistance Tests")]
    public void TestPersistanceOnLoadScene2() {
      Invoke(nameof(TestPersistance), .2f);
      UnloadSceneAsync(GetActiveScene());
      _scene2.OpenSingle(); // ! sometimes not close scene 1
    }

    [Button]
    [LabelText("On Scene 2 Additively Loaded")]
    [BoxGroup("Persistance Tests")]
    // ? always pass if TestUniquenessOnCurrentScene() fails
    public void TestPersistanceOnLoadScene2Additively() {
      Invoke(nameof(TestPersistance), .2f);
      _scene2.Open();
    }

    private void TestPersistance() {
      if (_singleton == null) {
        print("Failed");
      } else {
        print("Passed");
      }
    }
  }
}