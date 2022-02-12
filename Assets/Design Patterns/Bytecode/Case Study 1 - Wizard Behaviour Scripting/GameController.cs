using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Debug;

namespace BytecodePattern.Case1.Base1 {
  //Bytecode code pattern from the book "Game Programming Patterns"
  public class GameController : MonoBehaviour {
    [SerializeField] private List<SerializableInstruction> instructions = new List<SerializableInstruction>();

    private void Start() {
      var vm = new VirtualMachine(this);
      vm.Interpret(GetBytecode());
    }

    private int[] GetBytecode() {
      var bytecode = new List<int>();
      foreach (var instruction in instructions) bytecode.AddRange(instruction.ByteCode);

      return bytecode.ToArray();
    }

    //0 means the player's wizard and 1, 2, ... means the other wizards in the game
    //This way we can heal our own wizard while damage other wizards with the same method
    public void SetHealth(int wizardID, int amount) => Log($"Wizard {wizardID} gets health {amount}");

    public void SetWisdom(int wizardID, int amount) => Log($"Wizard {wizardID} gets wisdom {amount}");

    public void SetAgility(int wizardID, int amount) => Log($"Wizard {wizardID} gets agility {amount}");

    public void PlaySound(int soundID) => Log($"Play sound {soundID}");

    public void SpawnParticles(int particleType) => Log($"Spawn particle {particleType}");

    public int GetHealth(int wizardID) => 50;
  }
}