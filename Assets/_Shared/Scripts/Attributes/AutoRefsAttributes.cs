﻿// Credit: https://assetstore.unity.com/packages/tools/utilities/autorefs-109670
// Refactored by: enginooby

using System;

[Flags]
public enum AutoRefTarget {
  Undefined = 0,
  Self = 1,
  Parent = 2,
  Children = 4,
  Siblings = 8,
  Scene = 16,
  NamedGameObjects = 32,
}

[AttributeUsage(AttributeTargets.Field)]
public class AutoRefAttribute : Attribute {
  public readonly AutoRefTarget Target;
  public readonly string[] GameObjectNames; // find by names

  public AutoRefAttribute() => Target = AutoRefTarget.Self;

  public AutoRefAttribute(AutoRefTarget target, string[] targetNames = null) {
    Target = target;
    GameObjectNames = targetNames;
  }
}