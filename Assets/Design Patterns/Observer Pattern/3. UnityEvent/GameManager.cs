using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // !

namespace ObserverPattern3 {
  public class GameManager : MonoBehaviour {
    public UnityEvent<int> onLevelIncrease = new UnityEvent<int>(); // !
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
        onLevelIncrease.Invoke(_level); // !
      }
    }
  }
}