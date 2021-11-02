using UnityEngine;

namespace Digger
{
    public struct ModificationParameters
    {
        public Vector3 Position;
        public BrushType Brush;
        public ActionType Action;
        public int TextureIndex;
        public float Opacity;
        public float Size;
        public bool RemoveDetails;
        public bool RemoveTreesInSphere;
        public float StalagmiteHeight;
        public bool StalagmiteUpsideDown;
        public bool OpacityIsTarget;
    }
}