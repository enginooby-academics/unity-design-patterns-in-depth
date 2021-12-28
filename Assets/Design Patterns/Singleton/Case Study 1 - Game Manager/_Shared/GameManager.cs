using System.Collections;
using UnityEngine;

namespace SingletonPattern.Case1 {
  public class GameManager : MonoBehaviour {
    private int _level = 1;
    public int Level => _level;

    private void Start() {
      StartCoroutine(IncreaseLevelCourotine());
    }

    private IEnumerator IncreaseLevelCourotine() {
      while (true) {
        int randomPeriod = Random.Range(2, 5);
        yield return new WaitForSeconds(randomPeriod);
        Debug.Log("Current level: " + (++_level));
      }
    }
  }
}
