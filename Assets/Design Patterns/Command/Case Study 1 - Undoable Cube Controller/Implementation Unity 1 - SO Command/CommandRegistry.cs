using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CommandPattern.Case1.Unity1 {
  [CreateAssetMenu(fileName = "New Move Command Registry", menuName = "Patterns/Command/Move Command Registry", order = 0)]
  public class CommandRegistry : ScriptableObject {
    [SerializeField, InlineEditor]
    public List<MoveCommand> _commands = new List<MoveCommand>();
    public List<MoveCommand> Commands => _commands;
  }
}
