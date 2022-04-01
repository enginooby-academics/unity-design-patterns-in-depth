// TODO: Create wrapper for Scene class from ASM

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginooby.Attribute;
#endif

using System.Collections;
using System.Linq;
using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;

namespace SingletonPattern {
  /// <summary>
  ///   For testing singleton regarding: uniqueness (old is persistent, new is destroyed), persistence, laziness.
  ///   ! Replay after each test.
  /// </summary>
  // TIP: Customize script title in the Inspector
  [AddComponentMenu("SingletonPattern/MonoBehaviour Singleton Tester")]
  public class MonoBehaviourSingletonTester : MonoBehaviourSingleton<MonoBehaviourSingletonTester> {
    [SerializeField] private Scene _scene2;

#if ASM
    [SerializeField]
    [InfoBox(
      "ASM scene can be found in Settings/Resources with the same name of the scene. Scene also needs added in the Build setting.")]
    private AdvancedSceneManager.Models.Scene _scene2;
#endif

    [SerializeField] private GameObject _singleton;

    private readonly float TEST_DELAY = .2f;

    private string singletonName;

    private void Start() => singletonName = _singleton.name;

    [Button]
    [LabelText("In-Scene")]
    [HorizontalGroup("Split", 0.5f)]
    [BoxGroup("Split/Uniqueness Tests")]
    public void TestUniquenessOnCurrentScene() {
      Instantiate(_singleton);
      // ! add a small delay wait for new instance get destroyed
      Invoke(nameof(TestUniqueness), TEST_DELAY);
    }

    [Button]
    [LabelText("Across-Scene")]
    [BoxGroup("Split/Uniqueness Tests")]
    public void TestUniquenessOnLoadScene2Additively() {
      // ! temperory solution: add a small delay to ensure Testuniqueness is performed after new scene is opened.
      Invoke(nameof(TestUniqueness), TEST_DELAY);
      _scene2.LoadAdditively();
    }

    private void TestUniqueness() {
      var singletons = Resources.FindObjectsOfTypeAll<GameObject>()
        .Where(go => go.name.Contains(singletonName))
        .ToArray();

      print(singletons.Length > 1 ? "Failed" : "Passed");
    }

    [Button]
    [LabelText("Reload Scene")]
    [BoxGroup("Split/Persistence Tests")]
    public void TestPersistenceOnReloadCurrentScene() {
      StartCoroutine(TestPersistenceOnReloadCurrentSceneCoroutine());
      // alternative:
      // Invoke(nameof(TestPersistence), TEST_DELAY);
      // LoadScene(GetActiveScene().name);
    }

    public IEnumerator TestPersistenceOnReloadCurrentSceneCoroutine() {
      var operation = LoadSceneAsync(GetActiveScene().name);
      while (!operation.isDone) yield return null;
      TestPersistence();
    }

    [Button]
    [LabelText("Change Scene")]
    [BoxGroup("Split/Persistence Tests")]
    public void TestPersistenceOnLoadScene2() {
      Invoke(nameof(TestPersistence), TEST_DELAY);
      UnloadSceneAsync(GetActiveScene());
      _scene2.Load();
    }

    [Button]
    [LabelText("Add Scene")]
    [BoxGroup("Split/Persistence Tests")]
    // ? always pass if TestUniquenessOnCurrentScene() fails
    public void TestPersistenceOnLoadScene2Additively() {
      Invoke(nameof(TestPersistence), TEST_DELAY);
      _scene2.LoadAdditively();
    }

    private void TestPersistence() => print(_singleton == null ? "Failed" : "Passed");
  }
}