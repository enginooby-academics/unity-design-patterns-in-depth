using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern {
  public class Player : MonoBehaviour {
    private int _health;

    public void UpdateHealthOnNewLevel(int level) {
      _health = level * 2;
      Debug.Log("Player health: " + _health);
    }
  }
}