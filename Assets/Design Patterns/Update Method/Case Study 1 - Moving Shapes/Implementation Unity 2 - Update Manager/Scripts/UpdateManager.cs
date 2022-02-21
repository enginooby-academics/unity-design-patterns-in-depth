using System.Collections.Generic;
using Enginooby.Utils;
using UnityEngine;

namespace UpdateMethodPattern.Case1.Unity2 {
  // Example of custom update method to illustrate the update method pattern in the book "Game Programming Patterns"
  // This idea is based on code from the book "Unity 2017 Game Optimization"
  // This class will run all our custom update methods in Unity's own Update method, which will make it easier to pause the game
  public class UpdateManager : MonoBehaviourSingleton<UpdateManager> {
    private readonly List<IUpdatable> _updatables = new();
    private bool _isPaused;

    // This should be the game's only MonoBehaviour Update method
    private void Update() {
      if (!_isPaused) UpdateUpdatables();
      if (Input.GetKeyDown(KeyCode.Space)) _isPaused = !_isPaused;
    }

    private void UpdateUpdatables() {
      // Iterate through all objects backwards in case one object decides to destroy itself
      for (var i = _updatables.Count - 1; i >= 0; i--)
        _updatables[i]?.OnUpdate(Time.deltaTime);
    }

    public void RegisterUpdating(IUpdatable obj) => _updatables.AddIfNotContains(obj);

    public void UnregisterUpdating(IUpdatable obj) => _updatables.Remove(obj);
  }
}