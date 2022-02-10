using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

namespace StatePattern.Base {
  public class Environment : MonoBehaviourSingleton<Environment> {
    List<GameObject> _checkpoints = new List<GameObject>();
    [SerializeField] string checkpointTag = "Checkpoint";
    // TIP: use IReadOnlyList to prevent from invoking list.Clear() (for getter property, while its List private attribute still can)
    [ShowInInspector] public IReadOnlyList<GameObject> Checkpoints => _checkpoints;

    void Start() {
      _checkpoints.AddRange(GameObject.FindGameObjectsWithTag(checkpointTag));
      _checkpoints = _checkpoints.OrderByName();
    }
  }
}