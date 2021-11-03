using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Singleton.ThreadSafety {
  public class GameManager : GenericSingleton<GameManager> {
    private int _level = 1;
    public int Level => _level;

    private void Start() {
      StartCoroutine(IncreaseLevelCouroutine());
    }

    private IEnumerator IncreaseLevelCouroutine() {
      while (true) {
        int randomPeriod = Random.Range(2, 5);
        yield return new WaitForSeconds(randomPeriod);
        Debug.Log("Current level: " + (++_level));
      }
    }
  }
}
