using System.Collections;
using UnityEngine;

namespace SingletonPattern.Case1.Unity5 {
  public class GameManager : GenericSingleton<GameManager> {
    public int Level { get; private set; } = 1;

    private void Start() {
      StartCoroutine(IncreaseLevelCoroutine());
    }

    private IEnumerator IncreaseLevelCoroutine() {
      while (true) {
        var randomPeriod = Random.Range(2, 5);
        yield return new WaitForSeconds(randomPeriod);
        Debug.Log("Current level: " + ++Level);
      }
    }
  }
}