using System.Collections;
using UnityEngine;

namespace SingletonPattern.Case1.Unity5 {
  public class GameManager : GenericSingleton<GameManager> {
    private int _level = 1;
    public int Level => _level;

    private void Start() {
      StartCoroutine(IncreaseLevelCoroutine());
    }

    private IEnumerator IncreaseLevelCoroutine() {
      while (true) {
        int randomPeriod = Random.Range(2, 5);
        yield return new WaitForSeconds(randomPeriod);
        Debug.Log("Current level: " + (++_level));
      }
    }
  }
}
