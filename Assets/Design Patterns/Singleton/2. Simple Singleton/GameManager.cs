using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// + eager initialization
// + global access
// + scene-persistent: no
// + unique
namespace Singleton.Simple {
  public class GameManager : MonoBehaviour {
    #region Singleton Implementation
    public static GameManager Instance;

    private void Awake() {
      if (Instance) {
        Destroy(gameObject);
      } else {
        Instance = this;
      }
    }
    #endregion

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
