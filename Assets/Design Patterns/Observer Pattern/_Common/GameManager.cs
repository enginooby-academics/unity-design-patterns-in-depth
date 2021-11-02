using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern {
  public class GameManager : MonoBehaviour {
    public int level;

    void Start() {
      StartCoroutine(IncreaseLevelCouroutine());
    }

    IEnumerator IncreaseLevelCouroutine() {
      int randomPeriod = Random.Range(2, 5);
      yield return new WaitForSeconds(randomPeriod);
      level++;
      Debug.Log("Current level: " + level);
      StartCoroutine(IncreaseLevelCouroutine());
    }
  }
}