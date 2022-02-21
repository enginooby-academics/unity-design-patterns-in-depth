using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace BytecodePattern.Case1.Base1 {
  [Serializable]
  [InlineProperty]
  // To script instructions on the Inspector
  public class SerializableInstruction {
    [SerializeField] private Instruction instruction;

    [ShowIf(nameof(IsInstructionLiteral))] [SerializeField]
    private int literalValue;

    private bool IsInstructionLiteral => instruction == Instruction.Literal;

    public int[] ByteCode => IsInstructionLiteral ? new[] {(int) instruction, literalValue} : new[] {(int) instruction};
  }
}