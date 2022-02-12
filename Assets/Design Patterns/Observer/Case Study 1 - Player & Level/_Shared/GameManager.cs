using System.Collections;
using UnityEngine;

namespace ObserverPattern {
  public class GameManager : MonoBehaviour {
    [SerializeField] protected int _level;
    public int Level => _level;

    private void Start() {
      StartCoroutine(IncreaseLevelCouroutine());
    }

    private IEnumerator IncreaseLevelCouroutine() {
      while (true) {
        var randomPeriod = Random.Range(2, 5);
        yield return new WaitForSeconds(randomPeriod);
        Debug.Log("Current level: " + ++_level);
        OnLevelIncreasedCallback();
      }
    }

    protected virtual void OnLevelIncreasedCallback() {
    }
  }
}