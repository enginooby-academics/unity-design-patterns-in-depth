#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using System;

[Serializable, InlineProperty]
public class OnMouseDownEvent : TriggerEvent { }