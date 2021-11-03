# Singleton
## Study Case
+ Game manager can be used to store global data which has influence on many different objects in the game, such as current level. Hence, making it _**globally accessible**_ can ease the referecing process. In the other hand, the game manager needs to be _**unique**_ at any time to avoid data conflict, e.g., one player cannot be in two different levels at the same time.

#### 1. Naive approach: static
+ Marking instance static is an quick and easy way to achieve global accessibility. Howevers, this approach comes with many drawbacks:
  + The instance is not secured to be unique.
  + Only eager initialization.
  + Not scene-persistent.

#### 2. Simple singleton
+ In the start of scene loading, all instances other the first one are destroyed to keep it unique.