using System.Collections.Generic;
using UnityEngine;

namespace BytecodePattern.Case1.Base1 {
  public class VirtualMachine {
    private const int StackCapacity = 128;

    private readonly GameController _gameController;

    //Will store values for later use in the switch statement
    private readonly Stack<int> _stackMachine = new Stack<int>(StackCapacity);

    public VirtualMachine(GameController gameController) => _gameController = gameController;

    public void Interpret(int[] bytecode) {
      _stackMachine.Clear();

      //Read and execute the instructions
      for (var i = 0; i < bytecode.Length; i++) {
        //Convert from int to enum
        var instruction = (Instruction) bytecode[i];

        switch (instruction) {
          case Instruction.SetHealth: {
            //Important to pop amount before wizard because we push wizard before amount onto the stack
            var amount = _stackMachine.Pop();
            var wizard = _stackMachine.Pop();
            _gameController.SetHealth(wizard, amount);
            break;
          }
          case Instruction.Literal: {
            ////Important that this i++ is not inside bytecode[i++] or it will not jump to next i
            //i++;
            //int value = bytecode[i];
            //Push(value);

            //this can be a oneliner, bytecode will use i+1 bytecode element
            _stackMachine.Push(bytecode[++i]);
            break;
          }
          case Instruction.GetHealth: {
            var wizard = _stackMachine.Pop();
            _stackMachine.Push(_gameController.GetHealth(wizard));
            break;
          }
          case Instruction.Add: {
            var b = _stackMachine.Pop();
            var a = _stackMachine.Pop();
            _stackMachine.Push(a + b);
            break;
          }
          default: {
            Debug.Log($"The VM couldn't find the instruction {instruction} :(");
            break;
          }
        }
      }
    }
  }
}