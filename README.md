## Outline
To learn design patterns in a systematic way, we need a consistent format to present them. For each pattern, there are:
+ **Abstraction**: define the pattern in high concept based on its classification, intent & motivation.
+ **Skeleton**: structure or template for design pattern implementation.
+ **Case studies**: each propose a specific feature which can make use of the according design pattern. For each case study, we have:
  + **Naive implementation**: approach the proposed case study without using design pattern for comparative purposes.
  + **Base implementation**: basic and generic implementation of the design pattern. This implementation is not dependent on C# or Unity specific features, which means it can be easily transformed into any other OOP programming languages, in any tools (libraries/frameworks/engines). 
  + **C# implementation**: implementations dependent on C#-specific features (e.g. implement Observer Pattern using C# Action).
  + **Unity implementation**: implementations dependent on Unity-specific features (e.g. implement Observer Pattern using UnityEvent).
  + _Shared: folder containing common base scripts, assets for all naive approaches and implementations.
+ **Applications**: where and how the design pattern is used in Unity engine (e.g. Observer Pattern is used in UI EventSystem).
