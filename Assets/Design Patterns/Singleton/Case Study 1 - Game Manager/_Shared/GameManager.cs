using System.Collections;
using UnityEngine;

namespace SingletonPattern.Case1 {
  public class GameManager : MonoBehaviour {
    public int Level { get; private set; } = 1;

    private void Start() {
      StartCoroutine(IncreaseLevelCourotine());
    }

    private IEnumerator IncreaseLevelCourotine() {
      while (true) {
        var randomPeriod = Random.Range(2, 5);
        yield return new WaitForSeconds(randomPeriod);
        Debug.Log("Current level: " + ++Level);
      }
    }
  }
}