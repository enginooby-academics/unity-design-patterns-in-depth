using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// + eager initialization
// + global access
// + scene-persistent: no if access variable via static instance
// + unique: no
namespace Singleton.Static {
  public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    private int _level = 1;
    public int Level => _level;

    private void Awake() {
      Instance = this;
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