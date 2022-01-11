## What is this folder?
Basically - my custom library/framework containing reusable components for developing different projects in Unity engine. \
Specifically, there are many common base classes, serializable classes, extension methods, interfaces, Monobehaviours, component operators, etc.\

+ Base: base classes.
+ Graphics: post-processing related.
+ Wrapper (vs. Asset Preset?): add more functionalites (public methods, editor) to the wrapped type.
+ Component Operator: manipulate wrapped Unity component in the runtime.
+ GameObject Interactor: universal interactor which act in the same way for any GameObject (e.g. highlighter can highlight any GameObject).
+ Controller (vs. Asset Operator?)
+ Reference: serializable reference for in-scene entities (GameObject, Vector3, etc) and SO reference for value types (int, bool, etc) implementing with observer pattern.
+ Stat: variable w/ events and UIs. Two approaches - serializable class and scriptable object.
+ Event: two approaches - MonoBehaviour and scriptable object.
+ Area: continuous area & decrete area (containing points). Used for spawner, vision, formation, etc.
+ Input: wrapper for simple keycode & Input Manager with additional modifier keys.
+ Spawner: many modes - auto (by time period), active (by input), trigger (using collider), wave. Pool implemented.
+ Utils: extension methods.
+ Utilility/Plugin/Addon: modular systems.
+ Asset Data/Preset/Profile: ScriptableObject contains many configured data for an asset (e.g., CursorData configures texture, hotspot; AudioClipData contains tuned volume and pitch for an audio clip).
+ Asset Variation/Collection: ScriptableObject collection of multiple interchangable asset data for a situation (e.g., an AudioClipCollection contains different clips to play randomly when player gets attacked). This collection belongs to a GameObject.
+ Asset Theme/Context/Collection: ScriptableObject collection of multiple simultaneous asset data or asset data collections for a context/theme/project (e.g., many horror AudioClipCollection form a AudioClipTheme to be used in a horror game). This collection belongs to multiple GameObjects.
+ Collection (vs. Asset Variation?)
+ Switcher (vs. Asset Variation?)
+ Game Manager: basic, universal manager containing common stats (live, score, etc) and common events (start, pause, over, win) for quick prototyping.

Scripts are categorized by domains/features (UI, Graphics, Audio, Stats, etc) instead of language/file types (Enum, Interface, Serializable, ScriptableObject, etc). \

Vendors folder: 3rd-party assets.
+ TODO: isolate all vendor assets.

In addition to scripts, the Assets folder includes some of my favorite assets (free or self-made) such as audio, fonts, icons, post processing profiles.