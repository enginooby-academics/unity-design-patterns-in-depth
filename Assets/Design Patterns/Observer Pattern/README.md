# Observer Pattern
## Study Case
+ Game manager increases level after each random period of time. Each time level increase, player's health is set as equal to double of level.
+ Subject: game. Subject's state: level. 
+ Observer: player. Observer's action: update health.

#### 1. Naive approach: observer reference
+ In this approach, subject (game manager) keeps the reference of its observer (player). When subject state (level) changes, the reference invokes its action (update health).
+ Cons: tight coupling.

#### 2. Naive approach: subject tracking (update polling)
+ Observer keep reference to its subject and cache subject's state. Observer needs to keep tracking the state (caching validation) continously to detect when it changes to invoke the action.
+ Cons: high CPU usage.

#### 3. UnityEvent
+ Create an UnityEvent field for the observer. When state changes, UnityEvent is invoked which also invokes observer's action subscribing/binding to it. There are two ways to bind event:
  + Inspector event binding: drag the observer to into subject's inspector and choose the action.
  + Script event binding: from observer, make reference to subject's UnityEvent (or set event static and refer w/o the instance of subject class) and bind the action with ```AddListener()```.
+ Pros: loose coupling, visual event binding (in inspector).
+ Cons: poor security since observer can modify or invoke the UnityEvent of the observer.

#### 4. C# event: Action, Delegate & Func
+ Procedure is same as UnityEvent. C# events are not serializable, hence cannot bind actions via the inspector.
  + Action can not have return type.
  + Delegate & Func can have return type.
  + Actions should be binded to event in observer OnEnable() and removed in OnDisable().
+ Pros: loose coupling, more secured, higher performance than UnityEvent.
+ Cons: no visual event binding.


## Unity Features
#### UI Event System
Event cant be triggered by UI Event System when Graphic Raycaster hit an UI target.