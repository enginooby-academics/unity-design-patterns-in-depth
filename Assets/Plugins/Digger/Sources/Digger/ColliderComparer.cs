using System.Collections.Generic;
using UnityEngine;

namespace Digger
{
    public sealed class ColliderComparer : IEqualityComparer<Collider>
    {
        public bool Equals(Collider col1, Collider col2)
        {
            if (col1 == null && col2 == null)
                return true;

            if (col1 == null || col2 == null)
                return false;

            return col1.GetInstanceID() == col2.GetInstanceID();
        }

        public int GetHashCode(Collider col)
        {
            return col.GetHashCode();
        }
    }
}