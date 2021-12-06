## Case Study 1: Shape Calculators
Supposedly, we have implemented different type of procedural shapes (i.e cube, sphere, cylinder). Now we want to write algorithms to calculate some math figures of generated shapes (e.g, surface, area, volume). 

### Naive Aproaches
Calculating methods is written directly inside in each procedural shape class.
+ This violates SRP since procedural shape's responsibility should be generating shape only.
+ If we want to change how and which figure to calculate, changes need to be made in procedural shape's source code, which will violate OCP.

### Implementation Base
+ Visitable contract: ICalculatable
+ Visitor contract: ICalculator
+ Abstract (visitable) element: ProceduralShape implements ICalculatable
+ Concrete (visitable) elements: ProceduralCube, ProceduralSphere, ProceduralCylinder
+ Concrete visitors: SurfaceCalculator, AreaCalculator, VolumeCalculator implements ICalculator
+ Visitor manager: CalculatorManager - manage and switch between different calculators to demonstrate the flexibily of this pattern.
