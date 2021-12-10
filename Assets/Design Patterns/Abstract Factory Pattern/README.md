## Case Study 1: Theme Shape Factory
Implement a shape generator which creates different types of shape (i.e cube, sphere, cylinder) by theme (e.g. red shape, green shape, big shape, animated shape, etc).

### Naive Approach: Monolith Class
All theme parameters (color, size, animation, etc) are put inside ShapeGenerator class. Shape creation methods are implemented based on those parameters.
+ When new type of theme is added, the generator class has to be modified and becomes bigger. This violates OCP.
+ This class also violates SRP with both resposibilities of creating shapes and managing/switching between themes.

### Base Implementation: Pure Procedural
A base abstract factory is created which contains contract (abstract) creation methods for each shape type. Each theme class extending the base factory has to implement those contract methods.
+ Abstract factory: ShapeFactory,
+ Concrete factories: RedShapeFactory, GreenShapeFactory, BigShapeFactory, AnimatedShapeFactory.
+ Client class: ClientShapeGenerator.

### Unity Implementation: Pure Prefab
In some cases, all creation methods can be entirely imitated by prefabs, then all separated concrete factories are no longer needed. Instead, one common factory is created with serializable prefabs, then alternative to a concrete theme factory class, we create set of prefabs for that theme then assign them to an instance of factory prefab.

### Unity Implementation: Prefab & Procedural
If creation methods cannot be entirely imitated by prefabs, but only a part, then we can both use prefabs (for prefab-imitable code) and concrete factory classes (for prefab-unimitable code).

### Unity Implementation: ScriptableObject