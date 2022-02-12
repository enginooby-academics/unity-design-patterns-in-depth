namespace BytecodePattern.Case1.Base1 {
  //These are the instructions we can choose from in our programming language
  public enum Instruction {
    //Write stats
    SetHealth,
    SetWisdom,
    SetAgility,
    PlaySound,
    SpawnParticles,

    //So we can use parameters
    Literal,

    //Read stats
    GetHealth,
    GetWisdom,
    GetAgility,

    //Arithmetic
    Add
  }
}