using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

namespace Enginooby {
  [Serializable]
  [InlineProperty]
  public class OnTriggerEnterEvent : ColliderEvent { }
}