## What is this folder?
Basically - my custom library/framework containing reusable components for developing different projects in Unity engine. \
Specifically, there are many common base classes, serializable classes, extension methods, interfaces, Monobehaviours, component operators, etc.\

+ **Base**: base classes.
+ **Graphics**: post-processing related.
+ **Wrapper** (vs. Asset Preset?): add more functionalites (public methods, editor) to the wrapped type.
+ **Component Operator**: manipulate wrapped Unity component in the runtime.
+ **GameObject Interactor (GOI)**: universal interactor which act in the same way for any GameObject, hence every interactor class is Singleton (e.g. highlighter can highlight any GameObject). TODO: Nullify/extinguish by projectile, area, schedule; fade in/out effect
+ **GameObject Interator Controller**: wrapper for GameObject Interactor with control keys to perform interacting by input.
+ **Controller** (vs. Component Operator?)
+ **Reference & Variable**: serializable reference for in-scene entities (GameObject, position, etc) and SO reference for value types (int, bool, etc) implementing with observer pattern.
+ **Stat**: variable + events + UIs. Two approaches - serializable class and scriptable object.
+ **Event**: two approaches - MonoBehaviour and scriptable object.
+ **Area** ```Where things happen?```: continuous area & decrete area (containing points). Used for spawner, vision, formation, etc.
+ **Action Scheduler** ```"When things happen"```
+ **Input**: wrapper for simple keycode & Input Manager with additional modifier keys.
+ **Utils**: extension methods.
+ **Utilility/Plugin/Addon**: modular systems.
+ **Asset Profile**/Data/Preset: ScriptableObject contains many configured data for an asset (e.g., CursorData configures texture, hotspot; AudioClipData contains tuned volume and pitch for an audio clip).
+ **Asset Variation**/Collection (concurrent vs. un-concurrent?): ScriptableObject collection of multiple interchangable asset data for a situation (e.g., an AudioClipCollection contains different clips to play randomly when player gets attacked). This collection belongs to a GameObject.
+ **Asset Context**/Theme/Collection: ScriptableObject collection of multiple simultaneous asset data or asset data collections for a context/theme/project (e.g., many horror AudioClipVariation form a AudioClipContext to be used in a horror game). This collection belongs to multiple GameObjects.
+ **Collection** (vs. Asset Variation?)
+ **Switcher** (vs. Asset Variation?)
+ **Spawner**: many modes - auto (by time period), active (by input), trigger (using collider), wave. Pool implemented.
+ **Game Manager**: basic, universal manager containing common stats (live, score, etc) and common events (start, pause, over, win) for quick prototyping.

Scripts are categorized first by domain/feature (UI, Graphics, Audio, Stats, etc). Within each domain, scripts are categorized by language/type (Enum, Interface, Serializable, ScriptableObject, etc).\

Vendors folder: 3rd-party assets.
+ TODO: isolate all vendor assets.

In addition to scripts, the Assets folder includes some of my favorite assets (free or self-made) such as audio, fonts, icons, post processing profiles.