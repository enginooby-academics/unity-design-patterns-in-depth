## Case Study 1: Shape Builders
We will implement some similar types of builders to construct complex shapes consisting of procedural element/simple shapes (i.e cube, sphere, cylinder). Morever, the constructed shapes can also have visual effects or movement behaviours. These builders all make use of many subsystems/components/utilities such as procedural utility, visual effect system and assets, movement component, etc.

### Naive Approaches
Builders are implemented as irrelevant classes.
+ Builders may use same subsystems in similiar ways which results in duplicated code.
+ All builders are tighly coupled with subsystems they are using. If any subsystem changes, it may break many builders.

### Base Implementation
All concrete builders are subclasses of a base class Builder, which encapsulates common subsystems to implement necessary operations that all subclasses can invoke for shape construction. The base class also declears a Build() method contract that all concrete builders have to implement, which is also the shape construction process.
+ Since all concrete builder call common operations implemented in base class, code duplication is reduced.
+ Subsystem couplings are now all moved into one base class. If any subsystem change, only need change in the base class.