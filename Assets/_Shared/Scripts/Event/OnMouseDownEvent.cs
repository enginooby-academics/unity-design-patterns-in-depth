using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

[Serializable]
[InlineProperty]
public class OnMouseDownEvent : TriggerEvent { }