using UnityEngine;

namespace Digger.TerrainCutters
{
    public abstract class TerrainCutter : MonoBehaviour
    {
        public static TerrainCutter Create(Terrain terrain, DiggerSystem digger)
        {
            return TerrainCutter20193.CreateInstance(digger);
        }

        public static bool IsGoodVersion(TerrainCutter cutter)
        {
            return cutter is TerrainCutter20193;
        }

        private bool mustApply;
        private bool mustPersist;

        public void Apply(bool persist)
        {
            if (Application.isEditor && !Application.isPlaying) {
                ApplyInternal(persist);
            } else {
                mustApply = true;
                mustPersist = persist;
            }
        }

        private void Update()
        {
            if (mustApply) {
                ApplyInternal(mustPersist);
                mustApply = false;
                mustPersist = false;
            }
        }

        public abstract void Refresh();
        protected abstract void ApplyInternal(bool persist);
        public abstract void LoadFrom(string path);
        public abstract void SaveTo(string path);
        public abstract void Clear();
        public abstract void OnEnterPlayMode();
        public abstract void OnExitPlayMode();
    }
}