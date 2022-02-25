using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern.Case1.Unity1 {
  [CreateAssetMenu(fileName = "New Move Command Registry", menuName = "Patterns/Command/Move Command Registry",
    order = 0)]
  public class CommandRegistry : ScriptableObject {
    [SerializeField] private List<MoveCommand> _commands = new();
    public List<MoveCommand> Commands => _commands;
  }
}