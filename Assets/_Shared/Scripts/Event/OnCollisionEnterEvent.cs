using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Enginooby {
  [Serializable]
  [InlineProperty]
  public class OnCollisionEnterEvent : ColliderEvent { }
}