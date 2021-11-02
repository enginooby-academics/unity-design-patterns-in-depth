# Observer Pattern
## Study Case
+ Game manager increases level after each random period of time. Each time level increase, player's health is set as equal to double of level.
+ Subject: game. Subject's state: level. 
+ Observer: player. Observer's action: update health.

### 1. Naive approach: observer reference
+ In this approach, subject (game manager) keeps the reference of its observer (player). When subject state (level) changes, the reference invokes its action (update health).\
+ Cons: tight coupling.
