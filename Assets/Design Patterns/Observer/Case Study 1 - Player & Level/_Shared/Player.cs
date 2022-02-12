using UnityEngine;

namespace ObserverPattern {
  public class Player : MonoBehaviour {
    protected int _health;

    public virtual void UpdateHealthOnNewLevel(int level) {
      _health = level * 2;
      Debug.Log("Player health: " + _health);
    }
  }
}