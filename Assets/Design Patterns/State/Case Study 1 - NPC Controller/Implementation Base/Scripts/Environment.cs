using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

namespace StatePattern.Base {
  public class Environment : MonoBehaviourSingleton<Environment> {
    [SerializeField] private string checkpointTag = "Checkpoint";

    private List<GameObject> _checkpoints = new List<GameObject>();

    // TIP: use IReadOnlyList to prevent from invoking list.Clear() (for getter property, while its List private attribute still can)
    [ShowInInspector] public IReadOnlyList<GameObject> Checkpoints => _checkpoints;

    private void Start() {
      _checkpoints.AddRange(GameObject.FindGameObjectsWithTag(checkpointTag));
      _checkpoints = _checkpoints.OrderByName().ToList();
    }
  }
}