using UnityEngine;

namespace Digger
{
    public struct ModificationArea
    {
        public bool NeedsModification;
        public Vector3i Min, Max;
        public Vector3 OperationTerrainPosition;
    }
}