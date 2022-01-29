using System;
using System.Diagnostics;
using UnityEngine;

[Conditional("UNITY_CCU")]                                    // | This is necessary for CCU to pick up the right attributes
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class OptionalDependencyAttribute : Attribute        // | Must derive from System.Attribute
{
  public string dependentClass;                           // | Required field specifying the fully qualified dependent class
  public string define;                                   // | Required field specifying the define to add

  public OptionalDependencyAttribute(string dependentClass, string define) {
    this.dependentClass = dependentClass;
    this.define = define;
  }
}