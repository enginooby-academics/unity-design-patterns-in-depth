using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern1 {
  public class GameManager : MonoBehaviour {
    // ! Observer reference: tight coupling
    [SerializeField] ObserverPattern.Player player;

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
        player.UpdateHealthOnNewLevel(_level);
      }
    }
  }
}