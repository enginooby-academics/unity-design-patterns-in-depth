using UnityEngine;
using Sirenix.OdinInspector;
using AdvancedSceneManager.Models;
using System.Collections;
using static UnityEngine.SceneManagement.SceneManager;
using System.Linq;

namespace SingletonPattern {
  /// <summary>
  /// For testing singleton regarding: uniqueness (old is persistent, new is destroyed), persistence, laziness.
  /// ! Replay after each test.
  /// </summary>
  public abstract class SingletonTester<T> : MonoBehaviourSingleton<SingletonTester<T>> where T : class {
    private readonly float TEST_DELAY = .2f;
    [SerializeField]
    [InfoBox("ASM scene can be found in Settings/Resources with the same name of the scene. Scene also needs added in the Build setting.")]
    private Scene _scene2;

    private T _singleton;

    private string singletonName;

    private void Start() {
      // singletonName = _singleton.name;
    }

    [Button]
    [LabelText("In Current Scene")]
    [BoxGroup("Uniqueness Tests")]
    public void TestUniquenessOnCurrentScene() {
      System.Activator.CreateInstance(typeof(T), nonPublic: true);
      // ! add a small delay wait for new instance get destroyed
      Invoke(nameof(TestUniqueness), TEST_DELAY);
    }

    [Button]
    [LabelText("On Scene 2 Additively Loaded")]
    [BoxGroup("Uniqueness Tests")]
    public void TestUniquenessOnLoadScene2Additively() {
      // ! temperory solution: add a small delay to ensure Testuniqueness is performed after new scene is opened.
      Invoke(nameof(TestUniqueness), TEST_DELAY);
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
    [BoxGroup("Persistence Tests")]
    public void TestPersistenceOnReloadCurrentScene() {
      StartCoroutine(TestPersistenceOnReloadCurrentSceneCoroutine());
      // Invoke(nameof(TestPersistence), TEST_DELAY);
      // TestPersistence();
    }

    public IEnumerator TestPersistenceOnReloadCurrentSceneCoroutine() {
      var operation = LoadSceneAsync(GetActiveScene().name);
      while (!operation.isDone) yield return null;
      TestPersistence();
    }

    [Button]
    [LabelText("On Scene 2 Loaded")]
    [BoxGroup("Persistence Tests")]
    public void TestPersistenceOnLoadScene2() {
      Invoke(nameof(TestPersistence), TEST_DELAY);
      UnloadSceneAsync(GetActiveScene());
      _scene2.OpenSingle(); // ! sometimes not close scene 1
    }

    [Button]
    [LabelText("On Scene 2 Additively Loaded")]
    [BoxGroup("Persistence Tests")]
    // ? always pass if TestUniquenessOnCurrentScene() fails
    public void TestPersistenceOnLoadScene2Additively() {
      Invoke(nameof(TestPersistence), TEST_DELAY);
      _scene2.Open();
    }

    private void TestPersistence() {
      if (_singleton == null) {
        print("Failed");
      } else {
        print("Passed");
      }
    }
  }
}