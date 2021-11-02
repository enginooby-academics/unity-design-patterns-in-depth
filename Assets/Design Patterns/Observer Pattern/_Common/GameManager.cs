using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern {
  public class GameManager : MonoBehaviour {
    [SerializeField] private int _level;
    public int Level => _level;

    void Start() {
      StartCoroutine(IncreaseLevelCouroutine());
    }

    protected virtual IEnumerator IncreaseLevelCouroutine() {
      while (true) {
        int randomPeriod = Random.Range(2, 5);
        yield return new WaitForSeconds(randomPeriod);
        Debug.Log("Current level: " + (++_level));
      }
    }
  }
}