using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
using Enginooby.Core;
#endif

namespace CommandPattern.Case1.Unity1 {
  /// <summary>
  ///   * The 'SO Command' class
  /// </summary>
  [CreateAssetMenu(fileName = "New Move Command", menuName = "Patterns/Command/Move Command", order = 0)]
  public class MoveCommand : SerializedScriptableObject {
    [SerializeField] public KeyCode KeyCode { get; private set; }

    [SerializeField] [HorizontalGroup] public float XAmount { get; private set; }

    [SerializeField] [HorizontalGroup] public float YAmount { get; private set; }

    public bool CanExecute => Input.GetKeyDown(KeyCode);

    public void Execute(Cube cube) {
      cube.MoveX(XAmount);
      cube.MoveY(YAmount);
    }

    public void Undo(Cube cube) {
      cube.MoveX(-XAmount);
      cube.MoveY(-YAmount);
    }
  }
}